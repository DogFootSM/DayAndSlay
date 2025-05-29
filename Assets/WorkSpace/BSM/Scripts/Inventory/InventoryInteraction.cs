using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInteraction : InventoryController
{
    [SerializeField] private Item tempItem;

    private Dictionary<int, bool> ownedItemsDict = new Dictionary<int, bool>();
     
    /// <summary>
    /// 아이템 습득 후 인벤토리에 저장
    /// </summary>
    /// <param name="collectedItem"></param>
    public void AddItemToInventory(Item collectedItem)
    {
        if (ownedItemsDict.ContainsKey(collectedItem.ItemId)
            && collectedItem.itemData.IsOverlaped)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].CurSlotItem != null
                    && inventorySlots[i].CurSlotItem.itemData.IsOverlaped
                    && inventorySlots[i].CurSlotItem.itemData.ItemId == collectedItem.itemData.ItemId)
                {
                    inventorySlots[i].AddItem(collectedItem); 
                    //TODO: 아이템 습득 애니메이션 재생
                    return;
                }            
            }
        }

        bool emptySlots = inventorySlots.Where(x => x.CurSlotItem == null).Count() > 0;
        
        if (emptySlots)
        {
            inventorySlots.Where(x => x.CurSlotItem == null).First().AddItem(collectedItem);
            ownedItemsDict[collectedItem.ItemId] = true; 
            //TODO: 아이템 습득 애니메이션 재생
            return;
        }
        
        Debug.Log("빈 슬롯이 없습니다.");
        
    }
     
}
