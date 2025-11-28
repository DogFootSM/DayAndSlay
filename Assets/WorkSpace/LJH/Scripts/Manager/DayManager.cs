using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DayManager : MonoBehaviour, ISavable
{
    [SerializeField] private Image morning;
    [SerializeField] private Image afternoon;
    [SerializeField] private Image evening;

    [SerializeField] private GameObject taxUI;
    
    public static DayManager instance;

    public DayAndNight dayOrNight;

    // 9Minute
    private const int DefaultDayCount = 240;

    Coroutine timeCoroutine;

    [Inject] DataManager dataManager;
    [Inject] SqlManager sqlManager;
    [Inject] SaveManager saveManager;

    private bool isAfternoonStarted = false;
    private bool isEveningStarted = false;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartDay();
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

        StartDay();
    }
    
    private IEnumerator TaxRoutine()
    {
        // TODO: 세금 UI 노출
        taxUI.SetActive(true);
        yield return new WaitUntil(() => !taxUI.activeSelf);

        StartDay();
    }


    /// <summary>
    /// Method for StartDay
    /// NIGHT → DAY transition is managed by external conditions
    /// </summary>
    private void StartDay()
    {
        dayOrNight = DayAndNight.DAY;

        if (timeCoroutine == null)
        {
            timeCoroutine = StartCoroutine(TimeCoroutine());
        }
        //Todo : 밝아지고 상점 문이 열려야함


        //오전, 오후, 저녁 그림 초기값 세팅

        morning.color = new Color(morning.color.r, morning.color.g, morning.color.b, 1f);
        afternoon.color = new Color(afternoon.color.r, afternoon.color.g, afternoon.color.b, 0f);
        evening.color = new Color(evening.color.r, evening.color.g, evening.color.b, 0f);
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

            if (!isAfternoonStarted && elapsedGameTime >= DefaultDayCount * 2 / 3f)
            {
                StartCoroutine(FadeOutCoroutine(morning, afternoon));
                isAfternoonStarted = true;
            }

            if (!isEveningStarted && elapsedGameTime >= DefaultDayCount - (DefaultDayCount * 0.01f))
            {
                StartCoroutine(FadeOutCoroutine(afternoon, evening));
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
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        dayOrNight = DayAndNight.NIGHT;
        //Todo : 어두워지고 상점 문이 닫혀야함
    }

    /// <summary>
    /// 낮밤 저장
    /// </summary>
    /// <param name="sqlManager"></param>
    public void Save(SqlManager sqlManager)
    {
        sqlManager.UpdateCharacterDataColumn
        (new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.LAST_PLAYED_TIME) },
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
        string hourString = hour.ToString("D2");
        string minuteString = minute.ToString("D2");

        if (clockText != null)
        {
            clockText.text = $"{hourString}:{minuteString}";
        }
    }
}