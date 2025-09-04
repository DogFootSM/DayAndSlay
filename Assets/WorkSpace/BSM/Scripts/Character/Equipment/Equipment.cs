using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Equipment : MonoBehaviour
{
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EquipmentUI equipmentUI;
    
    private Dictionary<Parts, ItemData> equipSlotDict = new();
    
    /// <summary>
    /// 아이템 장착
    /// </summary>
    /// <param name="itemData">장착할 아이템 데이터</param>
    public void EquipItem(ItemData itemData)
    {
        Parts key = itemData.Parts;

        if (equipSlotDict.ContainsKey(key))
        {
            playerModel.ApplyItemModifiers(equipSlotDict[key], false); 
        }

        equipSlotDict[key] = itemData;
        
        //장착 아이템으로 UI 이미지 변경
        equipmentUI.OnChangeEquipItem?.Invoke(equipSlotDict[key], true);

        //장착 아이템 효과 스탯 반영
        playerModel.ApplyItemModifiers(equipSlotDict[key]);
        
        //장비 타입일 경우 Return
        if (equipSlotDict[key].WeaponType == WeaponType.NOT_WEAPON) return;
        
        //새로 착용한 무기 타입을 캐릭터에게 전달 
        playerController.ChangedWeaponType((CharacterWeaponType)equipSlotDict[key].WeaponType, itemData);
    }

    /// <summary>
    /// 아이템 장착 해제
    /// </summary>
    /// <param name="key">장착 해제할 아이템의 Key</param>
    public void UnEquipItem(Parts key)
    {
        if (equipSlotDict.ContainsKey(key))
        {
            equipmentUI.OnChangeEquipItem?.Invoke(equipSlotDict[key], false);
            playerModel.ApplyItemModifiers(equipSlotDict[key], false);
            playerController.ChangedWeaponType((CharacterWeaponType)WeaponType.NOT_WEAPON);
            equipSlotDict.Remove(key);
        } 
    } 
}