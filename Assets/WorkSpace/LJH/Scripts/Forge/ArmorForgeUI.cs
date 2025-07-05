using System.Collections.Generic;
using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;

public class ArmorForgeUI : BaseForgeUI<Parts, MaterialType>
{
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> armorStorage;

    protected override void Init()
    {
        // itemDict[Parts][MaterialType] = 아이템 리스트 초기화
        foreach (Parts part in GetUsableTabs())
        {
            itemDict[part] = new Dictionary<MaterialType, List<ItemData>>();

            foreach (MaterialType mat in Enum.GetValues(typeof(MaterialType)))
            {
                itemDict[part][mat] = LoadItems(part, mat);
            }
        }
    }

    private List<ItemData> LoadItems(Parts part, MaterialType mat)
    {
        List<ItemData> list = new();
        for (int i = 1; i <= DICTSIZE; i++)
        {
            string key = $"{part}_{mat}_{i}";
            if (armorStorage.TryGetValue(key, out var data))
                list.Add(data);
            else
                Debug.LogWarning($"[ArmorForgeUI] Item not found: {key}");
        }
        return list;
    }

    protected override void TabWrapperInit() 
    {
        tabButtonDictList.Add("헬멧", GetUI<Button>("HelmetTab"));
        tabButtonDictList.Add("갑옷", GetUI<Button>("ArmorTab"));
        tabButtonDictList.Add("바지", GetUI<Button>("PantsTab"));
        tabButtonDictList.Add("장갑", GetUI<Button>("ArmTab"));
        tabButtonDictList.Add("신발", GetUI<Button>("ShoesTab"));
    }
    protected override void TypeWrapperInit() 
    {
        tabButtonDictList.Add("중갑", GetUI<Button>("Type1"));
        tabButtonDictList.Add("가죽", GetUI<Button>("Type2"));
        tabButtonDictList.Add("천", GetUI<Button>("Type3"));
    }
    protected override void ItemWrapperInit()
    {
        itemButtonDictList.Add("아이템1", GetUI<Button>("Item1"));
        itemButtonDictList.Add("아이템2", GetUI<Button>("Item2"));
        itemButtonDictList.Add("아이템3", GetUI<Button>("Item3"));
        itemButtonDictList.Add("아이템4", GetUI<Button>("Item4"));
        itemButtonDictList.Add("아이템5", GetUI<Button>("Item5"));
    }

    protected override MaterialType[] GetUsableTypes()
    {
        return new MaterialType[] {
        MaterialType.PLATE,
        MaterialType.LEATHER,
        MaterialType.CLOTH
    };
    }
    protected override Parts GetDefaultTab() => Parts.HELMET;
    protected override MaterialType GetDefaultType() => MaterialType.PLATE;

    protected override Parts[] GetUsableTabs() =>
        new Parts[] { Parts.HELMET, Parts.ARMOR, Parts.ARM, Parts.PANTS, Parts.SHOES };
}