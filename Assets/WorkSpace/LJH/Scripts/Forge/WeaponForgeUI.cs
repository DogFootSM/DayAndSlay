using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;
using TMPro;

public class WeaponForgeUI : BaseForgeUI
{
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> weaponStorage;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> subWeaponStorage;

    [SerializeField] private List<ItemData> itemDataStorage = new List<ItemData>();
    [SerializeField] private List<ItemData> subItemStorage = new List<ItemData>();
    
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private SubWeaponType subWeaponType;

    protected override void StartInit()
    {
        itemDataStorage = ItemDatabaseManager.instance.GetNormalWeaponItem();
        subItemStorage = ItemDatabaseManager.instance.GetSubWeaponItem();

        // 리스트 정렬
        itemDataStorage = itemDataStorage
            .OrderBy(item => item.ItemId) // ItemId를 기준으로 오름차순 정렬
            .ToList();                    // 결과를 다시 List<ItemData>로 변환하여 할당

        subItemStorage = subItemStorage
            .OrderBy(item => item.ItemId)
            .ToList();
        
        /*
        List<int> weaponTempList = new List<int>();
        List<int> subweaponTempList = new List<int>();
        foreach (ItemData item in itemDataStorage)
        {
            weaponTempList.Add(item.ItemId);
        }
        foreach (ItemData item in subItemStorage)
        {
            subweaponTempList.Add(item.ItemId);
        }
        
        weaponTempList.Sort();
        subweaponTempList.Sort();
        
        itemDataStorage.Clear();
        subItemStorage.Clear();
        
        foreach (int id in weaponTempList)
        {
            itemDataStorage.Add(ItemDatabaseManager.instance.GetItemByID(id));
        }
        foreach (int id in subweaponTempList)
        {
            subItemStorage.Add(ItemDatabaseManager.instance.GetItemByID(id));
        }*/
    }

    protected override void SetTypeButton(Parts parts)
    {
        base.parts = parts;

        switch (parts)
        {
            case Parts.WEAPON:
                for (int i = 0; i < 4; i++)
                {
                    typeButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = ((WeaponType_kr)i).ToString();
                }
                break;

            case Parts.SUBWEAPON:
                for (int i = 0; i < 4; i++)
                {
                    typeButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = ((SubWeaponType_kr)i).ToString();
                }
                break;
        }
    }

    protected override void SetItemButton(int typeIndex)
    {
        if (parts == Parts.WEAPON)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((WeaponType)typeIndex == WeaponType.NOT_WEAPON)
                    break;

                ItemData itemData = weaponStorage[$"{(WeaponType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.Name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }

        }
        else if (parts == Parts.SUBWEAPON)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((SubWeaponType)typeIndex == SubWeaponType.NOT_SUBWEAPON)
                    break;

                ItemData itemData = subWeaponStorage[$"{(SubWeaponType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.Name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }
        }
    }


    protected override void ButtonInit()
    {
        //Binding Buttons
        for (int i = 0; i < 4; i++)
        {
            typeButtonList.Add(GetUI<Button>($"Type{i + 1}"));
        }

        for (int i = 0; i < 5; i++)
        {
            itemButtonList.Add(GetUI<Button>($"Item{i + 1}"));
        }

        for (int i = 0; i < 5; i++)
        {
            hideList.Add(GetUI($"Hide{i + 1}"));
        }

        //AddListener Buttons
        GetUI<Button>("WeaponTab").onClick.AddListener(() => Tap_TabButton(Parts.WEAPON));
        GetUI<Button>("SubWeaponTab").onClick.AddListener(() => Tap_TabButton(Parts.SUBWEAPON));

        for (int i = 0; i < typeButtonList.Count; i++)
        {
            int index = i;
            typeButtonList[i].onClick.AddListener(() => Tap_TypeButton(index));
        }

        for (int i = 0; i < itemButtonList.Count; i++)
        {
            int index = i;
            itemButtonList[i].onClick.AddListener(() => Tap_ItemButton(index));
        }

        //Initialized Buttons
        Tap_TabButton(Parts.WEAPON);
        Tap_TypeButton(defaultNum);
        
    }
}