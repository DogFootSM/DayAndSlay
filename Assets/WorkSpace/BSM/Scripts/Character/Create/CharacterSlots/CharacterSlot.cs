using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CharacterSlot : BaseUI
{
    [FormerlySerializedAs("slotIndex")] [HideInInspector] public int slotId;
    
    [Inject] private SqlManager sqlManager;
    [Inject] private CanvasManager canvasManager;
    [Inject] private DataManager dataManager;
    private GameObject selectPanelObj;
    private GameObject emptyText;
    private Button selectButton;
    private Button deleteButton;
    private IDataReader dataReader;
    private CharacterSlotController characterSlotController;
 
    private int isCreate;
 
    private void Start()
    {
        Bind();
        SelectSlotData(); 
        ButtonAddListener(); 
        CheckCharacterSlot();
    }

    private void CheckCharacterSlot()
    { 
        selectPanelObj.SetActive(isCreate != 0);
        emptyText.SetActive(isCreate == 0);  
    }

    /// <summary>
    /// 캐릭터 슬롯 패널 검색
    /// </summary>
    public void SelectSlotData()
    {         
        dataReader = sqlManager.ReadDataColumn(new []{"is_create"} ,new string[]{"slot_id"}, new string[]{$"{slotId}"},null);
        
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
        characterSlotController = GetComponentInParent<CharacterSlotController>();
        
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
        deleteButton.onClick.AddListener(DeleteCheck);
    }
    
    /// <summary>
    /// 캐릭터 슬롯 선택 동작
    /// </summary>
    private void CharacterSlotSelect()
    {
        if (isCreate == 0)
        {
            dataManager.SlotId = slotId;
            canvasManager.ChangeCanvas(MenuType.CHARACTER_CREATE);
        }
        else
        {
            Debug.Log("해당 캐릭터 데이터 로드");
        }
    }
    
    /// <summary>
    /// 삭제 진행 확인
    /// </summary>
    private void DeleteCheck()
    { 
        characterSlotController.DeleteSlot(slotId); 
    }
    
}
