using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DayManager : MonoBehaviour, ISavable
{
    [SerializeField] private Image sun;
    [SerializeField] private Image moon;
    
    
    public static DayManager instance;

    public DayAndNight dayOrNight;
    // 9Minute
    private const int DefaultDayCount = 54;
    private const int DayEndTime = 0;
    private int dayCount;
    private WaitForSeconds seconds = new WaitForSeconds(1f);

    Coroutine timeCoroutine;

    [Inject]DataManager dataManager;
    [Inject]SqlManager sqlManager;
    [Inject]SaveManager saveManager;
    
    public DayAndNight GetDayOrNight() => dayOrNight;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //saveManager.SavableRegister(this);

    }

    private void Start()
    {
        StartDay();
    }

    /// <summary>
    /// This method changes day & night
    /// </summary>
    public void SetDayNight()
    {
        if (dayOrNight == DayAndNight.DAY)
        {
            StartNight();
        }
        else if (dayOrNight == DayAndNight.NIGHT)
        {
            StartDay();
        }
    }

    private void SetDayCount(int value)
    {
        dayCount = value;

        if(dayCount <= DayEndTime && dayOrNight == DayAndNight.DAY)
        {
            SetDayNight();
        }
    }

    /// <summary>
    /// Method for StartDay
    /// NIGHT → DAY transition is managed by external conditions
    /// </summary>
    private void StartDay()
    {
        //태양 달 이미지 fillAmount 초기화
        sun.fillAmount = 1f;
        moon.fillAmount = 0f;
        
        SetDayCount(DefaultDayCount);
        dayOrNight = DayAndNight.DAY;
        
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
    IEnumerator TimeCoroutine()
    {
        while (dayOrNight == DayAndNight.DAY)
        {
            yield return seconds;
            dayCount--;

            sun.fillAmount -= 1f / DefaultDayCount;
            moon.fillAmount += 1f / DefaultDayCount;
            
            Debug.Log(sun.fillAmount);

            if(dayCount <= DayEndTime)
            {
                SetDayCount(DayEndTime);
            }
        }
    }

    /// <summary>
    /// Method For StartNight
    /// NIGHT → DAY transition is managed by external conditions
    /// </summary>
    private void StartNight()
    {
        if(timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }

        dayOrNight = DayAndNight.NIGHT;
        //Todo : 어두워지고 상점 문이 닫혀야함
    }

    public void Save(SqlManager sqlManager)
    {
        sqlManager.UpdateCharacterDataColumn
            (new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.LAST_PLAYED_TIME) },
            new[] { $"{dayOrNight}" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{dataManager.SlotId}"
            );
    }
}
