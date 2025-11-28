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
    [SerializeField] private Animator selectItemToastMessageAnimator;
    
    private InventoryInteraction inventoryInteraction;
    private Table targetTable;
    private ItemData itemToRegister;
    private InventorySlot removeInventorySlot;
    private Npc targetNpc;
    
    private int notRegisterToastMessageAnimHash;
    private int notWantToastMessageAnimHash;
    private int missingToastMessageAnimHash;
    
    private void Awake()
    {
        for (int i = 0; i < slotsParent.transform.childCount; i++)
        {
            registerTableSlots.Add(slotsParent.transform.GetChild(i).GetComponentInChildren<RegisterTableSlot>());
        }

        notRegisterToastMessageAnimHash = Animator.StringToHash("NotRegisterToastMessage");
        notWantToastMessageAnimHash = Animator.StringToHash("NotWantToastMessage");
        missingToastMessageAnimHash = Animator.StringToHash("MissingToastMessage");
    }

    /// <summary>
    /// 아이템 등록을 위한 팝업 활성화
    /// </summary>
    /// <param name="inventoryInteraction">캐릭터 인벤토리</param>
    /// <param name="target">아이템을 등록하기 위해 인식한 테이블</param>
    public void OpenRegisterItemPanel<T>(InventoryInteraction inventoryInteraction, T target)
    {
        this.inventoryInteraction = inventoryInteraction;

        if (target is Table table)
        {
            this.targetTable = table;
        }
        else if (target is StoreManager storeManager)
        {
            if (targetNpc == null)
            {
                return;
            } 
        }
        
        registerItemPanel.SetActive(true);

        //Slots에 인벤토리 아이템들 셋팅
        for (int i = 0; i < this.inventoryInteraction.GetSlotItemList().Item1.Count; i++)
        {
            registerTableSlots[i].SetRegisterItem(this.inventoryInteraction.GetSlotItemList().Item1[i],
                this.inventoryInteraction.GetSlotItemList().Item2[i]);
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
        if (itemToRegister == null)
        {
            selectItemToastMessageAnimator.SetTrigger(notRegisterToastMessageAnimHash);
            return;
        }

        if (targetNpc != null)
        {
            if (targetNpc.wantItem == null)
            {
                selectItemToastMessageAnimator.SetTrigger(missingToastMessageAnimHash);
                OnPlayerExitRangeClosePanel();
                return;
            }
            
            if (targetNpc.wantItem.ItemId != itemToRegister.ItemId)
            {
                selectItemToastMessageAnimator.SetTrigger(notWantToastMessageAnimHash);
                return;
            } 
        }
        
        if (targetTable != null)
        {
            targetTable.TakeItem(itemToRegister); 
        }
        removeInventorySlot.RemoveItem();
        OnPlayerExitRangeClosePanel();
    }

    /// <summary>
    /// 플레이어가 범위를 벗어날 경우 아이템 등록 판넬 종료
    /// </summary>
    public void OnPlayerExitRangeClosePanel()
    {
        for (int i = 0; i < registerTableSlots.Count; i++)
        {
            registerTableSlots[i].ResetRegisterItem();
        }
        registerTableUI.OnRegisterUIResetEvents?.Invoke();
        
        inventoryInteraction = null;
        targetTable = null;
        itemToRegister = null;
        removeInventorySlot = null;
        targetNpc = null;
        registerItemPanel.SetActive(false);
    }
}