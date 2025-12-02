using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CharacterSlot : BaseUI
{
    [HideInInspector] public int slotId;
    [SerializeField] private List<Image> characterSlotImages;
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI debtText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    [Inject] private SqlManager sqlManager;
    [Inject] private CanvasManager canvasManager;
    [Inject] private DataManager dataManager;
    private GameObject selectPanelObj;
    private GameObject emptyText;
    private Button selectButton;
    private Button deleteButton;
    private IDataReader dataReader;
    private CharacterSlotController characterSlotController;
    private SoundManager soundManager => SoundManager.Instance;

    private int isCreate;
    private int currentDay;
    private int debt;
    private int level;

    //0  없음
    //1 ~ 20만 빚이 조금 있음
    //20만 1 ~ 40만 빚이 있음
    //40만 1 ~ 60만 빚이 조금 많음
    //60만 1 ~ 80만 빚이 많음
    //80 ~ 빚이 매우 많음
    private string[] debtRange = new string[]
    {
        "빚이 조금 있음", "빚이 있음", "빚이 조금 많음", "빚이 많음", "빚이 매우 많음",
    };
    
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
                sqlManager.GetCharacterColumn(CharacterDataColumns.IS_CREATE)
            }, 
            new string[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
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
            sqlManager.GetCharacterColumn(CharacterDataColumns.HAIR_SPRITE),
            sqlManager.GetCharacterColumn(CharacterDataColumns.BODY_SPRITE),
            sqlManager.GetCharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
            sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_SPRITE), 
        }, new string[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID)}, 
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
        
        dataReader = sqlManager.ReadDataColumn(new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.CURRENTDAY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.DEBT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CHAR_LEVEL),
            }, new string[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID)}, 
            new string[] { $"{slotId}" }, 
            null);

        while (dataReader.Read())
        {
            currentDay = dataReader.GetInt32(0);
            debt = dataReader.GetInt32(1);
            level = dataReader.GetInt32(2);
        }
        
        

        if (debt == 0)
        {
            debtText.text = "빚이 없음";
        }
        else
        {
            int index = (debt / 200000) > 4 ? 4 : debt / 200000;
            debtText.text = debtRange[index];
        }
        
        currentDayText.text = $"{currentDay}일차"; 
        levelText.text = $"Lv.{level}"; 
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
            soundManager.PlaySfx(SFXSound.UI_BUTTON_CLICK);
            canvasManager.ChangeCanvas(CanvasType.CHARACTER_CREATE);
        }
        else
        {
            soundManager.PlaySfx(SFXSound.GAMESTART);
            characterSlotController.LoadInGameScene();
        }
    }

    /// <summary>
    /// 삭제 진행 확인
    /// </summary>
    private void DeleteCheck()
    {
        soundManager.PlaySfx(SFXSound.UI_BUTTON_CLICK);
        characterSlotController.DeleteSlot(slotId);
    }
}