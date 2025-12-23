using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;

public class PlayerRoot : MonoBehaviour
{
    public static PlayerRoot PlayerRootInstance;

    [Inject] private DataManager dataManager;
    [Inject] private SqlManager sqlManager;
    
    private DayManager dayManager => DayManager.instance;
    private int currentTimeState = 0;
    
    private void Awake()
    {
        if (PlayerRootInstance == null)
        {
            PlayerRootInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        ProjectContext.Instance.Container.Inject(this);
        SetDayOrNightState(); 
    }
  
    public void TranslateScenePosition(Vector2 position)
    {
        transform.position = position;
    }

    private void OnDisable()
    {
        CoolDownUIHub.CoolDownImageMap.Clear();
        CoolDownUIHub.BuffCoolDownMap.Clear();
    }

    /// <summary>
    /// DB에서 저장 시점을 불러옴
    /// 저장 시점에 따라 DayManager Morning or Night 설정
    /// </summary>
    private void SetDayOrNightState()
    {
        IDataReader reader = sqlManager.ReadDataColumn(
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.CURRENTTIME) },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) }, new[] { $"{dataManager.SlotId}" },
            null);
        
        while (reader.Read())
        {
            currentTimeState = reader.GetInt32(0);
        }

        switch (currentTimeState)
        {
            case 0:
                dayManager.StartMorning();
                break;
            
            case 1:
                dayManager.StartNight();
                break;
        } 
    }
    
}
