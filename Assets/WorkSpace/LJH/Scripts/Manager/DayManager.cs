using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DayManager : MonoBehaviour, ISavable
{
    [SerializeField] private NpcSpawner npcSpawner;
    
    [SerializeField] private Image morning;
    [SerializeField] private Image day;
    [SerializeField] private Image evening;
    [SerializeField] private Image night;

    [SerializeField] private GameObject taxUI;
    [SerializeField] private GameObject wantItemList;

    [SerializeField] private SpriteRenderer nightFilter;
    
    public static DayManager instance;

    public DayAndNight dayOrNight;

    // 9Minute
    [SerializeField] private  int DefaultDayCount = 480;

    Coroutine timeCoroutine;

    [Inject] DataManager dataManager;
    [Inject] SqlManager sqlManager;
    [Inject] SaveManager saveManager;

    private bool isdayStarted = false;
    private bool isEveningStarted = false;
    private bool isNightStarted = false;

    private bool isMorning = true;

    /// <summary>
    ///  시계 영역
    /// </summary>
    ///
    ///
    private int startHour = 9;

    [SerializeField] private TextMeshProUGUI clockText;

    private int lastTotalMinutes = -1;
    private int hour;
    private int minute;

    public DayAndNight GetDayOrNight() => dayOrNight;

    /// <summary>
    /// 아침,낮,밤 설정
    /// </summary>
    /// <param name="dn">DayAndNight enum값</param>
    public void SetDayOrNight(DayAndNight dn)
    {
        SetAllAlphaToZero(); // 일단 모두 투명하게 만듭니다.

        switch (dn)
        {
            case DayAndNight.MORNING:
                morning.color = new Color(morning.color.r, morning.color.g, morning.color.b, 1f);
                break;
            case DayAndNight.DAY:
                day.color = new Color(day.color.r, day.color.g, day.color.b, 1f);
                break;
            case DayAndNight.NIGHT:
                night.color = new Color(night.color.r, night.color.g, night.color.b, 1f);
                break;
        }

        dayOrNight = dn;
    }
    
    private void SetAllAlphaToZero()
    {
        morning.color = new Color(morning.color.r, morning.color.g, morning.color.b, 0f);
        day.color = new Color(day.color.r, day.color.g, day.color.b, 0f);
        evening.color = new Color(evening.color.r, evening.color.g, evening.color.b, 0f);
        night.color = new Color(night.color.r, night.color.g, night.color.b, 0f);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(DungeonManager.hasDungeonEntered)
            StartMorning();
        
        else
            StartNight();
        
        UpdateClockDisplay(8, 0);
    }

    public void OpenStore()
    {
        StartCoroutine(FlowToNineCoroutine());
    }
    
    //아침 시작시 8시 > 9시까지 흐르게 해주는 코루틴
    private IEnumerator FlowToNineCoroutine()
    {
        isMorning = false;
        
        float clockMinutes = 8 * 60; // 8시 시작 → 480분
        float target = 9 * 60;       // 9시 → 540분

        while (clockMinutes < target)
        {
            // 빠른 진행 (원하면 속도 조절 가능)
            clockMinutes += Time.deltaTime * 60f; 
            // 600배속 → 60초 정도면 1시간 흐름

            int hour = (int)(clockMinutes / 60);
            int minute = (int)(clockMinutes % 60);

            UpdateClockDisplay(hour, minute);

            yield return null;
        }

        // 9시 도달 보정
        UpdateClockDisplay(9, 0);

        // 낮 시작되며 엔피씨 스폰 시작
        StartDay();
        StartCoroutine(npcSpawner.NpcSpawnCoroutine());

    }


    /// <summary>
    /// 낮 ↔ 밤 상태를 전환
    /// </summary>
    public void ToggleDayNight()
    {
        if (dayOrNight == DayAndNight.DAY)
        {
            StartNight();
        }
        else
        {
            PreDayEvents(); // 아침 시작 전 이벤트 처리
        }
    }
    
    private void PreDayEvents()
    {
        IngameManager.instance.AddDay();

        if (IngameManager.instance.IsTaxDay())
        {
            StartCoroutine(TaxRoutine());
            return;
        }

        StartMorning();
    }
    
    private IEnumerator TaxRoutine()
    {
        // TODO: 세금 UI 노출
        taxUI.SetActive(true);
        yield return new WaitUntil(() => !taxUI.activeSelf);

        StartMorning();
    }

    private void SetNightFilterAlpha(float alpha)
    {
        Color filterCol = nightFilter.color;
        filterCol.a = alpha;
        nightFilter.color = filterCol;
    }

    /// <summary>
    /// 저장 및 던젼 종료시 호출
    /// </summary>
    public void StartMorning()
    {
        SetNightFilterAlpha(0);
        SetDayOrNight(DayAndNight.MORNING);
        isMorning = true;
    }


    /// <summary>
    /// Method for StartDay
    /// NIGHT → DAY transition is managed by external conditions
    /// </summary>
    private void StartDay()
    {
        wantItemList.SetActive(true);
        SetDayOrNight(DayAndNight.DAY);

        if (timeCoroutine == null)
        {
            timeCoroutine = StartCoroutine(TimeCoroutine());
        }
        //Todo : 밝아지고 상점 문이 열려야함
    }

    /// <summary>
    /// TimeControlled Coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeCoroutine()
    {
        float elapsedGameTime = 0f;
        float elapsedClockTime = 0f;

        float gameClockScale = 540f / DefaultDayCount;

        while (dayOrNight == DayAndNight.DAY)
        {
            // 실제 게임 시간은 1배속으로 누적
            elapsedGameTime += Time.deltaTime;
            // 게임 내 시계 시간은 gameClockScale 배속으로 누적
            elapsedClockTime += Time.deltaTime * gameClockScale;
            
            int currentTotalMinutes = Mathf.FloorToInt(elapsedClockTime);
            if (currentTotalMinutes != lastTotalMinutes)
            {
                lastTotalMinutes = currentTotalMinutes;

                int currentHour = (currentTotalMinutes / 60 + startHour) % 24;
                int currentMinute = currentTotalMinutes % 60;

                UpdateClockDisplay(currentHour, currentMinute);
            }

            if (!isdayStarted && elapsedGameTime >= DefaultDayCount * 2 / 3f)
            {
                StartCoroutine(FadeOutCoroutine(morning, day));
                isdayStarted = true;
            }

            if (!isEveningStarted && elapsedGameTime >= DefaultDayCount - (DefaultDayCount * 0.01f))
            {
                StartCoroutine(FadeOutCoroutine(day, evening));
                isEveningStarted = true;
            }

            if (elapsedGameTime >= DefaultDayCount)
            {
                ToggleDayNight();
                break;
            }

            yield return null; // 매 프레임 대기
        }
    }

    /// <summary>
    /// 시간대 이미지 페이드 아웃, 인 코루틴
    /// </summary>
    /// <param name="pre"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    private IEnumerator FadeOutCoroutine(Image pre, Image next)
    {
        yield return null;

        Color preStartColor = pre.color;
        Color nextStartColor = next.color;

        Color preEndColor = new Color(preStartColor.r, preStartColor.g, preStartColor.b, 0f);
        Color nextEndColor = new Color(preEndColor.r, preEndColor.g, preEndColor.b, 1f);

        //페이드 아웃에 걸리는 시간
        float duration = DefaultDayCount * 0.01f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            pre.color = Color.Lerp(preStartColor, preEndColor, t);
            next.color = Color.Lerp(nextStartColor, nextEndColor, t);

            yield return null;
        }

        // 최종 보정
        pre.color = preEndColor;
        next.color = nextEndColor;
    }

    /// <summary>
    /// Method For StartNight
    /// NIGHT → DAY transition is managed by external conditions
    /// </summary>
    private void StartNight()
    {
        //남아있는 npc가 있을 경우 삭제 해줌
        foreach (Npc npc in npcSpawner.GetNpcList())
        {
            npc?.NpcGone();
        }

        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        SetDayOrNight(DayAndNight.NIGHT);
        SetNightFilterAlpha(0.7f);
        //Todo : 어두워지고 상점 문이 닫혀야함
        
        morning.color = new Color(1,1,1,0);
        day.color = new Color(1,1,1,0);
        evening.color = new Color(1,1,1,0);
        night.color = new Color(1,1,1,1);
    }

    /// <summary>
    /// 낮밤 저장
    /// </summary>
    /// <param name="sqlManager"></param>
    public bool Save(SqlManager sqlManager)
    {
        return sqlManager.UpdateCharacterDataColumn
        (new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.GOLD) },
            new[] { $"{dayOrNight}" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{dataManager.SlotId}"
        );
    }
    
    /// <summary>
    /// 시계 UI에 표시
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="minute"></param>
    private void UpdateClockDisplay(int hour, int minute)
    {
        string ampm = (hour >= 12) ? "오후" : "오전";
    
   
        int displayHour;

        if (hour == 0)
        {
            displayHour = 12;
        }
        else if (hour > 12) 
        {
            displayHour = hour - 12;
        }
        else
        {
            displayHour = hour;
        }

        string hourString = displayHour.ToString("D2");
        string minuteString = minute.ToString("D2");

        if (clockText != null)
        {
            clockText.text = $"{ampm} {hourString}:{minuteString}";
        }
    }
}