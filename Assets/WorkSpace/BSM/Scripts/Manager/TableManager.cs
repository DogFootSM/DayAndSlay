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
    private InteractableObj table;

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
    /// <param name="table">아이템을 등록하기 위해 인식한 테이블</param>
    public void OpenRegisterItemPanel(InventoryInteraction inventoryController, InteractableObj table)
    {
        this.inventoryController = inventoryController;
        this.table = table;
        registerItemPanel.SetActive(true);
        
        Debug.Log($"{inventoryController.GetSlotItemList().Count} / {registerTableSlots.Count}");
        
        //Slots에 인벤토리 아이템들 셋팅
        for (int i = 0; i < this.inventoryController.GetSlotItemList().Count; i++)
        {
            registerTableSlots[i].SetRegisterItem(this.inventoryController.GetSlotItemList()[i]);
        }
    }

    public void SelectRegisterTableSlot(ItemData itemData)
    {
        //UI에 선택한 아이템 정보 갱신
    }
    
    
}
