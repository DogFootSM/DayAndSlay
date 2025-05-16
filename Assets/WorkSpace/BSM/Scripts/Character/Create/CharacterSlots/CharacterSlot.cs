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
    [HideInInspector] public int slotIndex;
    
    [Inject] private SqlManager sqlManager;
    [Inject] private CanvasManager canvasManager;
    private GameObject selectPanelObj;
    private GameObject emptyText;
    private Button selectButton;
    private Button deleteButton;
    private IDataReader dataReader;
    
 
    private int isCreate;
 
    private void Start()
    {
        Bind();
        Init(); 
        ButtonAddListener(); 
        CheckCharacterSlot();
    }

    private void CheckCharacterSlot()
    { 
        selectPanelObj.SetActive(isCreate != 0);
        emptyText.SetActive(isCreate == 0);  
    }

    /// <summary>
    /// 캐릭터 슬롯 패널 초기화
    /// </summary>
    private void Init()
    {         
        dataReader = sqlManager.ReadDataColumn(new []{"is_create"} ,new string[]{"slot_id"}, new string[]{$"{slotIndex}"},null);
        
        while (dataReader.Read())
        {
            isCreate = dataReader.GetInt32(0); 
        }
    }
    
    /// <summary>
    /// UI 요소 바인딩
    /// </summary>
    private void Bind()
    {
        selectButton = GetComponent<Button>();
        
        selectPanelObj = GetUI("CharacterSelectPanel");
        emptyText = GetUI("EmptyText");
        deleteButton = GetUI<Button>("DeleteButton");
    }

    /// <summary>
    /// 버튼 이벤트 등록
    /// </summary>
    private void ButtonAddListener()
    {
        selectButton.onClick.AddListener(CharacterSlotSelect);
        deleteButton.onClick.AddListener(CharacterDelete);
    }
    
    /// <summary>
    /// 캐릭터 슬롯 선택 동작
    /// </summary>
    private void CharacterSlotSelect()
    {
        if (isCreate == 0)
        {
            canvasManager.ChangeCanvas(MenuType.CHARACTER_CREATE);
        }
        else
        {
            Debug.Log("해당 캐릭터 데이터 로드");
        }
    }

    private void CharacterDelete()
    { 
        sqlManager.UpdateDataColumn(sqlManager.ColumnNames, sqlManager.DefaultValue, "slot_id", $"{slotIndex}");
      
    }
    
}
