using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] private DataManager dataManager;
    [Inject] private SqlManager sqlManager;
    
    private GameManager gameManager => GameManager.Instance;
    
    private List<ISavable> savables = new List<ISavable>();
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    /// <summary>
    /// 저장할 데이터 추상화 객체 저장
    /// </summary>
    /// <param name="savable">저장할 데이터 객체</param>
    public void SavableRegister(ISavable savable)
    {
        if (!savables.Contains(savable))
        {
            savables.Add(savable);
        }
    }

    /// <summary>
    /// 각각의 데이터 저장 로직 호출
    /// </summary>
    public bool GameDataSave()
    {
        bool success = true;
        
        foreach (var savable in savables)
        {
            if (!savable.Save(sqlManager))
            {
                success = false;
                break;
            } 
        }
  
        if (success)
        {
            gameManager.HasUnsavedChanges = false;

            if (DayManager.instance.GetDayOrNight() == DayAndNight.NIGHT)
            {
                DayManager.instance.StartMorning();
            }
            else if(DayManager.instance.GetDayOrNight() == DayAndNight.MORNING)
            {
                DayManager.instance.StartNight();
            } 
        } 
        
        return success;
    }
 
}