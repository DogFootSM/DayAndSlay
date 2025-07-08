using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Zenject;

public class DayManager : MonoBehaviour, ISavable
{
    public static DayManager instance;

    public DayAndNight dayOrNight;
    // 9Minute
    const int DefaultDayCount = 540;
    const int DayEndTime = 0;
    private int dayCount;
    private WaitForSeconds seconds = new WaitForSeconds(1f);

    Coroutine timeCoroutine;

    [Inject]DataManager dataManager;
    [Inject]SqlManager sqlManager;
    [Inject]SaveManager saveManager;

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

        saveManager.SavableRegister(this);

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
