using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentSlot
{
    public ItemData itemData;
    public InventorySlot inventorySlot;
}

public class Equipment : MonoBehaviour
{
    private Dictionary<Parts, EquipmentSlot> equipSlotDict = new ();

    public void ChangeEquipment(ItemData itemData, InventorySlot inventorySlot)
    {
        if (equipSlotDict.TryGetValue(itemData.parts, out EquipmentSlot equipSlot))
        { 
            equipSlot.inventorySlot.IsEquip = false; 
        }

        EquipmentSlot newEquipmentSlot = new EquipmentSlot()
        {
            itemData = itemData,
            inventorySlot = inventorySlot
        };
        
        inventorySlot.IsEquip = true;
        
        equipSlotDict[itemData.parts] = newEquipmentSlot; 
    } 
}
