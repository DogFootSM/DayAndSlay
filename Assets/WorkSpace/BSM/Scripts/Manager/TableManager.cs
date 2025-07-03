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
    
    private InventoryInteraction inventoryInteraction;
    private Table targetTable; 
    private ItemData itemToRegister;
    private InventorySlot removeInventorySlot;
    
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
    /// <param name="inventoryInteraction">캐릭터 인벤토리</param>
    /// <param name="targetTable">아이템을 등록하기 위해 인식한 테이블</param>
    public void OpenRegisterItemPanel(InventoryInteraction inventoryInteraction, Table targetTable)
    {
        this.inventoryInteraction = inventoryInteraction;
        this.targetTable = targetTable;
        registerItemPanel.SetActive(true); 
        
        //Slots에 인벤토리 아이템들 셋팅
        for (int i = 0; i < this.inventoryInteraction.GetSlotItemList().Item1.Count; i++)
        {
            registerTableSlots[i].SetRegisterItem(this.inventoryInteraction.GetSlotItemList().Item1[i], this.inventoryInteraction.GetSlotItemList().Item2[i]);
        }
    }

    /// <summary>
    /// 등록되어 있는 아이템을 다시 꺼냄
    /// </summary>
    public void WithdrawItem(InventoryInteraction inventoryInteraction, Table targetTable)
    {
        this.targetTable = targetTable;

        if (this.targetTable == null) return;
        
        this.targetTable.GiveItem(inventoryInteraction); 
    }
    
    /// <summary>
    /// 테이블 아이템 등록 슬롯에서 넘겨 받은 아이템 데이터를 기반으로 선택 아이템 UI 갱신
    /// </summary>
    /// <param name="itemData"></param>
    public void SelectRegisterTableSlot(ItemData itemData, InventorySlot inventorySlot)
    {
        itemToRegister = itemData;
        removeInventorySlot = inventorySlot;
        registerTableUI.OnRegisterItemEvents?.Invoke(itemData);
    }
    
    /// <summary>
    /// 선택한 아이템을 테이블 오브젝트에 등록
    /// </summary>
    public void Register()
    {
        targetTable.TakeItem(itemToRegister);
        registerItemPanel.SetActive(false); 
        removeInventorySlot.RemoveItem();
    }
    
}
