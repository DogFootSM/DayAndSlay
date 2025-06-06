using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Equipment : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    private Dictionary<Parts, EquipmentSlot> equipSlotDict = new();
    
    public void EquipItem(ItemData itemData, InventorySlot inventorySlot)
    {
        Parts key = itemData.Parts;

        if (equipSlotDict.TryGetValue(key, out EquipmentSlot equipSlot))
        {
            equipSlot.inventorySlot.IsEquip = false;
        }

        EquipmentSlot equipmentSlot = new EquipmentSlot()
        {
            itemData = itemData,
            inventorySlot = inventorySlot,
        };

        inventorySlot.IsEquip = true;
        
        equipSlotDict[key] = equipmentSlot;
        
    }

    public void UnEquipItem(Parts key)
    {
        if (equipSlotDict.TryGetValue(key, out EquipmentSlot equipSlot))
        {
            equipSlot.inventorySlot.IsEquip = false; 
            
            equipSlotDict.Remove(key);
        }
         
    }

    private void Update()
    {
        if (equipSlotDict.ContainsKey(Parts.WEAPON))
        {
            Debug.Log($"현재 장착중인 아이템:{equipSlotDict[Parts.WEAPON].itemData.Name}");
        }
        else
        {
            Debug.Log("장착 아이템 없음");
        }
    }
}