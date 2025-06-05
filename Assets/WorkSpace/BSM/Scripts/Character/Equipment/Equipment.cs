using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Equipment : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    private Dictionary<Parts, EquipmentSlot> equipSlotDict = new();
    
    public void ChangeEquipment(ItemData itemData, InventorySlot inventorySlot)
    {
        Parts key = itemData.parts;

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
}