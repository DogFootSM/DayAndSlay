using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Equipment : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private EquipmentUI equipmentUI;
    
    private Dictionary<Parts, EquipmentSlot> equipSlotDict = new();
    
    /// <summary>
    /// 아이템 장착
    /// </summary>
    /// <param name="itemData">장착할 아이템 데이터</param>
    /// <param name="inventorySlot">장착 아이템을 가지고 있는 인벤토리 슬롯</param>
    public void EquipItem(ItemData itemData, InventorySlot inventorySlot)
    {
        Parts key = itemData.Parts;
        
        //이미 아이템이 장착되어 있을 경우 기존 아이템 장착 해제
        if (equipSlotDict.TryGetValue(key, out EquipmentSlot equipSlot))
        {
            equipSlot.inventorySlot.IsEquip = false;
        }

        EquipmentSlot equipmentSlot = new EquipmentSlot()
        {
            itemData = itemData,
            inventorySlot = inventorySlot,
        };

        //슬롯의 아이템 장착 상태 변경
        inventorySlot.IsEquip = true;
        
        equipSlotDict[key] = equipmentSlot;
        
        //장착 아이템으로 UI 이미지 변경
        equipmentUI.OnChangeEquipItem?.Invoke(equipSlotDict[key]);
        
        //장착 아이템 효과 스탯 반영
        playerModel.ApplyItemModifiers(equipSlotDict[key].itemData);
    }

    /// <summary>
    /// 아이템 장착 해제
    /// </summary>
    /// <param name="key">장착 해제할 아이템의 Key</param>
    public void UnEquipItem(Parts key)
    {
        if (equipSlotDict.TryGetValue(key, out EquipmentSlot equipSlot))
        {
            equipSlot.inventorySlot.IsEquip = false;  
            equipmentUI.OnChangeEquipItem?.Invoke(equipSlotDict[key]);
            playerModel.ApplyItemModifiers(equipSlotDict[key].itemData, false);
            equipSlotDict.Remove(key);
        } 
    } 
}