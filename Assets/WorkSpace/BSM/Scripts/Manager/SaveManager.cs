using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] private DataManager dataManager;
    [Inject] private SqlManager sqlManager;
 
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
    public void GameDataSave()
    {
        foreach (var savable in savables)
        {
            savable.Save(sqlManager);
        } 
    }
 
}