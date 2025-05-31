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
    [HideInInspector] public int slotId;
    [SerializeField] private List<Image> characterSlotImages;

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
        IsCreatedCharacter();
        LoadSlotData();
        ButtonAddListener(); 
    }
 
    /// <summary>
    /// 캐릭터 생성 여부 확인
    /// </summary>
    public void IsCreatedCharacter()
    {
        dataReader = sqlManager.ReadDataColumn(new[]
            {
                sqlManager.CharacterColumn(CharacterDataColumns.IS_CREATE)
            }, 
            new string[] { sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID) },
            new string[] { $"{slotId}" },
            null);

        isCreate = 0;
        
        //캐릭터가 생성되어 있다면 1로 변경
        while (dataReader.Read())
        {
            isCreate = dataReader.GetInt32(0); 
        }
         
        selectPanelObj.SetActive(isCreate != 0);
        emptyText.SetActive(isCreate == 0);
    }
    
    /// <summary>
    /// 캐릭터 슬롯 패널 검색
    /// </summary>
    private void LoadSlotData()
    { 
        dataReader = sqlManager.ReadDataColumn(new[]
        {
            sqlManager.CharacterColumn(CharacterDataColumns.HAIR_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.BODY_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.WEAPON_SPRITE)
        }, new string[] { sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID)}, 
            new string[] { $"{slotId}" }, 
            null);

        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                characterSlotImages[i].sprite =
                    Resources.Load<Sprite>($"Preset/{(CharacterPresetType)i}/{dataReader.GetString(i)}");
            }
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
        dataManager.SlotId = slotId;

        if (isCreate == 0)
        {
            canvasManager.ChangeCanvas(CanvasType.CHARACTER_CREATE);
        }
        else
        {
            characterSlotController.LoadInGameScene();
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