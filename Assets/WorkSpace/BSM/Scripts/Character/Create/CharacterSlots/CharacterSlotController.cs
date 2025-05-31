using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CharacterSlotController : MonoBehaviour
{
    [SerializeField] private List<CharacterSlot> characterSlots;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private SceneReference inGameScene;
    [Inject] private SqlManager sqlManager;
    
    public GameObject DeleteAlert;
    public int DeleteSlotId;
    
    private readonly string[] ColumnNames = new[]
    {
        "is_create",
        "hair_sprite",
        "body_sprite",
        "shirt_sprite",
        "weapon_sprite",
        "last_played_time",
        "weapon_type",
        "remaining_days",
        "strength",
        "agility",
        "intelligence",
        "objective_item"
    };

    private readonly string[] DefaultValue = new[]
    {
        "0",
        "none",
        "none",
        "none",
        "none",
        "none",
        "0",
        "0",
        "0",
        "0",
        "0",
        "none"
    };
    
    protected void Awake()
    {
        for (int i = 0; i < transform.childCount -1; i++)
        {
            characterSlots[i].slotId = i + 1;
        }
        
        cancelButton.onClick.AddListener(() => DeleteAlert.SetActive(false)); 
        confirmButton.onClick.AddListener(DeleteSlotConfirm);
    }
    
    /// <summary>
    /// 삭제 얼럿 활성화
    /// </summary>
    /// <param name="slotId">삭제할 데이터 슬롯 id</param>
    public void DeleteSlot(int slotId)
    {
        DeleteSlotId = slotId;
        DeleteAlert.SetActive(true);
    }

    /// <summary>
    /// 슬롯 삭제 완료
    /// </summary>
    private void DeleteSlotConfirm()
    { 
        DeleteAlert.SetActive(false);
        
        //TODO: DELETE TABLE로 변경하기
        sqlManager.UpdateDataColumn(ColumnNames, DefaultValue, 
            sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID), 
            $"{DeleteSlotId}");
            
        characterSlots[DeleteSlotId - 1].IsCreatedCharacter();
    }
    
    /// <summary>
    /// 슬롯 선택 시 게임 씬 이동
    /// </summary>
    public void LoadInGameScene()
    {
        SceneManager.LoadScene(inGameScene.Name);
    }
    
}