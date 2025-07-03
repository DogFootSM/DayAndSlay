using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class RegisterTableSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    
    [Inject] private TableManager tableManager;
    
    private ItemData itemData;
    private InventorySlot inventoryToTableSlot;
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }
    
    /// <summary>
    /// 인벤토리에서 넘겨 받은 데이터들을 각 슬롯에 셋팅
    /// </summary>
    /// <param name="itemData">슬롯이 보유하고 있을 아이템 데이터</param>
    public void SetRegisterItem(ItemData itemData, InventorySlot inventorySlot)
    {
        this.itemData = itemData;
        itemImage.sprite = itemData.ItemImage; 
        inventoryToTableSlot = inventorySlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData == null) return;
        
        //선택한 아이템 전달
        tableManager.SelectRegisterTableSlot(itemData, inventoryToTableSlot);
    }
}
