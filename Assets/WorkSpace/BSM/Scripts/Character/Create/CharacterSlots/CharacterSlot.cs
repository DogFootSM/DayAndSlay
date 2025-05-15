using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterSlot : BaseUI
{
    [Inject] private SqlManager sqlManager;

    private IDataReader dataReader;
    
    private Button characterSelectButton; 
    private GameObject characterSelectPanel;
    private GameObject emptyText;
    
    private List<int> slots = new List<int>(3);
    
    private void Start()
    {
        Bind();
        MakeCharacterSlotCheck(); 
    }

    private void Bind()
    {
        characterSelectButton = GetComponent<Button>();
        characterSelectPanel = GetUI("CharacterSelectPanel");
        emptyText = GetUI("EmptyText"); 
    }
    
    /// <summary>
    /// 캐릭터 슬롯 데이터 존재 여부 확인
    /// </summary>
    private void MakeCharacterSlotCheck()
    {
        dataReader = sqlManager.ReadDataColumn("is_create");

        while (dataReader.Read())
        {
            slots.Add(dataReader.GetInt32(0));
        }

        for (int i = 0; i < slots.Count; i++)
        {
            characterSelectPanel.SetActive(slots[i] == 1); 
            emptyText.SetActive(slots[i] == 0); 
        }
    }
    
    
}
