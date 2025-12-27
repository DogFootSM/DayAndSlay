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

    public UnityAction<ItemData, bool> OnChangeEquipItem;

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
    private void EquipItemUI(ItemData newItem, bool isEquip)
    {
        Parts key = newItem.Parts;
        
        //아이템 장착, 해제 여부에 따른 슬롯 이미지 활성화, 비활성화
        partsUIDict[key].gameObject.SetActive(isEquip);
        
        //장착 시 아이템 이미지 할당
        if (isEquip)
        {
            partsUIDict[key].sprite = newItem.ItemImage;
        } 
    }
}
