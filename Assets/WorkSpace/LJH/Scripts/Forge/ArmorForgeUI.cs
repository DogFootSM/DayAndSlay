using System.Collections.Generic;
using System;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.UI;
using TMPro;

public class ArmorForgeUI : BaseForgeUI
{
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> helmetStorage;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> armorStorage;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> pantsStorage;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> armStorage;
    [SerializeField][SerializedDictionary] private SerializedDictionary<string, ItemData> shoesStorage;



    [SerializeField] private MaterialType materialType;


    protected override void SetTypeButton(Parts parts)
    {
        base.parts = parts;
        for (int i = 0; i < (int)MaterialType.CLOTH; i++)
        {
            typeButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = ((MaterialType_kr)i).ToString();
        }
    }

    protected override void SetItemButton(int typeIndex)
    {
        if (parts == Parts.HELMET)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData itemData = helmetStorage[$"{(MaterialType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }

        }
        else if (parts == Parts.ARMOR)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData itemData = armorStorage[$"{(MaterialType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }
        }
        else if (parts == Parts.PANTS)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData itemData = pantsStorage[$"{(MaterialType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }
        }
        else if (parts == Parts.ARM)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData itemData = armStorage[$"{(MaterialType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }
        }
        else if (parts == Parts.SHOES)
        {
            for (int i = 0; i < 3; i++)
            {
                ItemData itemData = shoesStorage[$"{(MaterialType)typeIndex}{i + 1}"];
                itemButtonList[i].GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
                itemButtonList[i].GetComponent<ItemButton>().itemData = itemData;
            }
        }
    }


    protected override void ButtonInit()
    {
        //Binding Buttons
        for (int i = 0; i < 3; i++)
        {
            typeButtonList.Add(GetUI<Button>($"Type{i + 1}"));
        }
        for (int i = 0; i < 5; i++)
        {
            itemButtonList.Add(GetUI<Button>($"Item{i + 1}"));
        }

        //AddListener Buttons
        GetUI<Button>("HelmetTab").onClick.AddListener(() => Tap_TabButton(Parts.HELMET));
        GetUI<Button>("ArmorTab").onClick.AddListener(() => Tap_TabButton(Parts.ARMOR));
        GetUI<Button>("PantsTab").onClick.AddListener(() => Tap_TabButton(Parts.PANTS));
        GetUI<Button>("ArmTab").onClick.AddListener(() => Tap_TabButton(Parts.ARM));
        GetUI<Button>("ShoesTab").onClick.AddListener(() => Tap_TabButton(Parts.SHOES));

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
        Tap_TabButton(Parts.ARMOR);
        Tap_TypeButton(defaultNum);

    }
}