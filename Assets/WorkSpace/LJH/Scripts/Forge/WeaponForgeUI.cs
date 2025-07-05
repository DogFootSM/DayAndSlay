using System.Collections.Generic;
using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;

public class WeaponForgeUI : BaseForgeUI<Parts, WeaponType>
{
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> weaponStorage;

    protected override void Init()
    {
        // itemDict[Parts][WeaponType] = 아이템 리스트 초기화
        foreach (Parts part in GetUsableTabs())
        {
            itemDict[part] = new Dictionary<WeaponType, List<ItemData>>();

            foreach (WeaponType weaponType in Enum.GetValues(typeof(WeaponType)))
            {
                if (weaponType == WeaponType.NOT_WEAPON)
                {
                    break;
                }
                itemDict[part][weaponType] = LoadItems(part, weaponType);
            }
        }
    }

    private List<ItemData> LoadItems(Parts part, WeaponType type)
    {
        List<ItemData> list = new();
        for (int i = 1; i <= DICTSIZE; i++)
        {
            string key = $"{type}{i}";
            if (weaponStorage.TryGetValue(key, out var data))
                list.Add(data);
            else
                Debug.LogWarning($"[WeaponForgeUI] Item not found: {key}");
        }
        return list;
    }

    protected override void TabWrapperInit() 
    {
        /* 탭 버튼 Wrapper 초기화 */
        tabButtonDictList.Add("무기", GetUI<Button>("WeaponTab"));
        tabButtonDictList.Add("보조무기", GetUI<Button>("SubWeaponTab"));
    }
    protected override void TypeWrapperInit() 
    {
        typeButtonDictList.Add("검", GetUI<Button>("Type1"));
        typeButtonDictList.Add("창", GetUI<Button>("Type2"));
        typeButtonDictList.Add("활", GetUI<Button>("Type3"));
        typeButtonDictList.Add("완드", GetUI<Button>("Type4"));
    }
    protected override void ItemWrapperInit()
    {
        itemButtonDictList.Add("아이템1", GetUI<Button>("Item1"));
        itemButtonDictList.Add("아이템2", GetUI<Button>("Item2"));
        itemButtonDictList.Add("아이템3", GetUI<Button>("Item3"));
        itemButtonDictList.Add("아이템4", GetUI<Button>("Item4"));
        itemButtonDictList.Add("아이템5", GetUI<Button>("Item5"));
    }

    protected override WeaponType[] GetUsableTypes()
    {
        return new WeaponType[] {
        WeaponType.SHORT_SWORD,
        WeaponType.SPEAR,
        WeaponType.BOW,
        WeaponType.WAND,
    };
    }
    protected override Parts GetDefaultTab() => Parts.WEAPON;
    protected override WeaponType GetDefaultType() => WeaponType.SHORT_SWORD;

    protected override Parts[] GetUsableTabs() => new Parts[] { Parts.WEAPON, Parts.SUBWEAPON };
}