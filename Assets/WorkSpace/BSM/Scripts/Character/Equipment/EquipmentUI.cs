using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializedDictionary("Parts Type", "Parts UI Object")] [SerializeField]
    private SerializedDictionary<Parts, Image> partsUIDict;

    public UnityAction<EquipmentSlot> OnChangeEquipItem;

    private void OnEnable()
    {
        OnChangeEquipItem += EquipItemUI;
    }

    private void OnDisable()
    {
        OnChangeEquipItem -= EquipItemUI;
    }

    /// <summary>
    /// 장착 아이템에 따른 이미지 변경
    /// </summary>
    /// <param name="equipmentSlot"></param>
    private void EquipItemUI(EquipmentSlot equipmentSlot)
    {
        Parts key = equipmentSlot.itemData.Parts;

        //장착과 장착 해제
        if (equipmentSlot.inventorySlot.IsEquip)
        {
            partsUIDict[key].sprite = equipmentSlot.itemData.ItemImage;
        }
        else
        {
            partsUIDict[key].sprite = null;
        }
        
    }
}
