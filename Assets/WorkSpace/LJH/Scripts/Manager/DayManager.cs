using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Zenject;

public class DayManager : MonoBehaviour, ISavable
{
    [SerializeField] private NpcSpawner npcSpawner;

    /// <summary>
    /// 오늘이 며칠인지
    /// </summary>
    [SerializeField] private int currentDay;

    [SerializeField] private Image morning;
    [SerializeField] private Image day;
    [SerializeField] private Image evening;
    [SerializeField] private Image night;

    [SerializeField] private GameObject taxUI;
    [SerializeField] private GameObject wantItemList;
    [SerializeField] private Volume townSceneVolume;

    private float morningIntensity = 0.75f;
    private float dayIntensity = 1f;
    private float nightIntensity = 0.8f;
    private float intensityIntervel = 0.1f;
    private List<ITorchSwitch> torches = new List<ITorchSwitch>();

    public static DayManager instance;

    public DayAndNight dayOrNight;

    // 9Minute
    [SerializeField] private int DefaultDayCount = 480;

    Coroutine timeCoroutine;

    [Inject] DataManager dataManager;
    [Inject] SqlManager sqlManager;
    [Inject] SaveManager saveManager;

    private bool isdayStarted = false;
    private bool isEveningStarted = false;
    private bool isNightStarted = false;

    private bool isMorning = true;

    private int currentDayTime = 0; //현재 시간이 새벽인지, 밤인지 구분 0 == 낮, 1 == 밤

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

    private ColorAdjustments ca;
    
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

        saveManager.SavableRegister(this);
    }
    
    /// <summary>
    /// 아침,낮,밤 설정
    /// </summary>
    /// <param name="dn">DayAndNight enum값</param>
    private void SetDayOrNight(DayAndNight dn)
    {
        SetAllAlphaToZero(); // 일단 모두 투명하게 만듭니다.

        Debug.Log($"현재 상태 :{dn}");
        foreach (var torch in torches)
        {
            torch.TorchSwitch(dn == DayAndNight.NIGHT);
        }

        switch (dn)
        {
            case DayAndNight.MORNING:
                morning.color = new Color(morning.color.r, morning.color.g, morning.color.b, 1f);
                townSceneVolume.profile.TryGet(out ca);
                ca.active = true;
                ca.colorFilter.value = new Color(0.6f, 0.6f, 0.6f);
                break;

            case DayAndNight.DAY:
                day.color = new Color(day.color.r, day.color.g, day.color.b, 1f);
                townSceneVolume.profile.TryGet(out ca);
                ca.active = true;
                ca.colorFilter.value = new Color(1f, 1f, 1f);
                break;

            case DayAndNight.NIGHT:
                night.color = new Color(night.color.r, night.color.g, night.color.b, 1f);
                townSceneVolume.profile.TryGet(out ca);
                ca.active = true;
                ca.colorFilter.value = new Color(0.33f, 0.33f, 0.33f);

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

    public void OpenStore()
    {
        if (dayOrNight == DayAndNight.MORNING) ToggleDayNight();
    }

    //아침 시작시 8시 > 9시까지 흐르게 해주는 코루틴
    private IEnumerator FlowToNineCoroutine()
    {
        isMorning = false;

        float clockMinutes = 8 * 60; // 8시 시작 → 480분
        float target = 9 * 60; // 9시 → 540분

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
        else if (dayOrNight == DayAndNight.MORNING)
        {
            PreDayEvents(); // 아침 시작 전 이벤트 처리
        }
    }

    private void PreDayEvents()
    {
        if (IngameManager.instance.IsTaxDay())
        {
            StartCoroutine(TaxRoutine());
            return;
        }

        StartCoroutine(FlowToNineCoroutine());
    }

    private IEnumerator TaxRoutine()
    {
        taxUI.SetActive(true);
        yield return new WaitUntil(() => !taxUI.activeSelf);

        StartCoroutine(FlowToNineCoroutine());
    }

    /// <summary>
    /// 저장 및 던젼 종료시 호출
    /// </summary>
    public void StartMorning()
    {
        GameManager.Instance.HasUnsavedChanges = true;
        currentDayTime = 0;
        UpdateClockDisplay(8, 0);
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
    public void StartNight()
    {
        currentDayTime = 1;
        UpdateClockDisplay(21, 0);

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

        morning.color = new Color(1, 1, 1, 0);
        day.color = new Color(1, 1, 1, 0);
        evening.color = new Color(1, 1, 1, 0);
        night.color = new Color(1, 1, 1, 1);
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

    /// <summary>
    /// 횃불 인터페이스 lIST 등록
    /// </summary>
    /// <param name="torch"></param>
    public void TorchRegister(ITorchSwitch torch)
    {
        if (!torches.Contains(torch))
        {
            torches.Add(torch);
        }
    }

    /// <summary>
    /// History : 2025.12.19
    /// 작성자 : 백선명
    /// 변경 내용 : 저장 성공 여부 체크 후 낮, 밤 상태 변경
    /// 낮밤 저장
    /// </summary>
    /// <param name="sqlManager"></param>
    public bool Save(SqlManager sqlManager)
    {
        if (GetDayOrNight() == DayAndNight.NIGHT)
        {
            IngameManager.instance.AddDay();
            StartMorning();
        }
        else if(GetDayOrNight() == DayAndNight.MORNING)
        {
            StartNight();
        }
        
        bool isSuccess = sqlManager.UpdateCharacterDataColumn
        (new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.CURRENTTIME) },
            new[] { $"{currentDayTime}" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{dataManager.SlotId}"
        );
        
        return isSuccess;
    }
}