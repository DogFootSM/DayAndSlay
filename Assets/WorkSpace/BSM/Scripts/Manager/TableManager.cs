using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    private List<RegisterTableSlot> registerTableSlots = new();
    
    [SerializeField] private RegisterTableUI registerTableUI;
    [SerializeField] private GameObject registerItemPanel;
    [SerializeField] private GameObject slotsParent;
    
    private InventoryInteraction inventoryController;
    private InteractableObj interactorTableObject;
    private Table targetTable; 
    private ItemData itemToRegister;
    
    private void Awake()
    { 
        for (int i = 0; i < slotsParent.transform.childCount; i++)
        {
            registerTableSlots.Add(slotsParent.transform.GetChild(i).GetComponentInChildren<RegisterTableSlot>());
        }
    }

    /// <summary>
    /// 아이템 등록을 위한 팝업 활성화
    /// </summary>
    /// <param name="inventoryController">캐릭터 인벤토리</param>
    /// <param name="interactorTableObject">아이템을 등록하기 위해 인식한 테이블</param>
    public void OpenRegisterItemPanel(InventoryInteraction inventoryController, InteractableObj interactorTableObject)
    {
        this.inventoryController = inventoryController;
        this.interactorTableObject = interactorTableObject;
        registerItemPanel.SetActive(true);

        targetTable = this.interactorTableObject as Table;
        
        //Slots에 인벤토리 아이템들 셋팅
        for (int i = 0; i < this.inventoryController.GetSlotItemList().Count; i++)
        {
            registerTableSlots[i].SetRegisterItem(this.inventoryController.GetSlotItemList()[i]);
        }
    }

    /// <summary>
    /// 등록되어 있는 아이템을 다시 꺼냄
    /// </summary>
    public void WithdrawItem()
    {
        //아이템을 꺼내게 되면 Table의 Item은 Null 로 변경
        //아이템을 꺼내서 Inventory에 다시 추가
        
    }
    
    /// <summary>
    /// 테이블 아이템 등록 슬롯에서 넘겨 받은 아이템 데이터를 기반으로 선택 아이템 UI 갱신
    /// </summary>
    /// <param name="itemData"></param>
    public void SelectRegisterTableSlot(ItemData itemData)
    {
        itemToRegister = itemData;
        
        //UI에 선택한 아이템 정보 갱신
        registerTableUI.OnRegisterItemEvents?.Invoke(itemData);
    }
    
    /// <summary>
    /// 선택한 아이템을 테이블 오브젝트에 등록
    /// </summary>
    public void Register()
    {
        targetTable.Interaction(itemToRegister);
        registerItemPanel.SetActive(false);
    }
    
}
