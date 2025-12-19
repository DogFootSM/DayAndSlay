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
    /// History : 2025.12.17
    /// 작성자 : 이재호
    /// 현재 진행일자 추가해주는 AddDay 메서드 여기에 추가
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
        } 
        
        return success;
    }
 
}