using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Forge UI의 공통 Base 클래스
/// Category = 탭 버튼 Enum (ex. Parts)
/// T = 타입 버튼 Enum (ex. MaterialType)
/// </summary>
public abstract class BaseForgeUI<Parts, T> : BaseUI
    where Parts : Enum
    where T : Enum
{
    [Header("Animator")]
    [SerializeField] protected Animator animator;

    [Header("UI Elements")]
    [SerializeField] protected DictList<Button> tabButtonDictList = new();
    [SerializeField] protected DictList<Button> typeButtonDictList = new();
    [SerializeField] protected DictList<Button> itemButtonDictList = new();
    [SerializeField] protected Image prevItemImage;
    [SerializeField] protected DictList<TextMeshProUGUI> prevTextDictList = new();
    [SerializeField] protected CreateButton createButton;

    protected const int DICTSIZE = 3;

    /// <summary>
    /// [Category][Type] => ItemData 리스트
    /// ex. [Parts.HELMET][MaterialType.PLATE] => 해당 아이템들
    /// </summary>
    protected Dictionary<Parts, Dictionary<T, List<ItemData>>> itemDict = new();
    protected List<ButtonWrapper> itemButtonWrappers = new();

    protected Parts currentCategory;

    // 상속 클래스에서 구현해야 하는 초기화 함수들
    protected abstract void Init();
    protected abstract void TabWrapperInit();
    protected abstract void TypeWrapperInit();
    protected abstract void ItemWrapperInit();
    protected abstract T[] GetUsableTypes();
    protected abstract Parts GetDefaultTab();
    protected abstract T GetDefaultType();
    protected abstract Parts[] GetUsableTabs();

    protected virtual void Start()
    {
        Init();
        TabWrapperInit();
        SetTabButtons();
        TypeWrapperInit();
        ItemWrapperInit();
        SetTypeButtons(GetDefaultTab());
        SetItemButtons(GetDefaultTab(), GetDefaultType());
    }

    /// <summary>
    /// 탭 버튼 초기화
    /// </summary>
    protected virtual void SetTabButtons()
    {
        var tabs = GetUsableTabs();
        int index = 0;

        foreach (Parts tab in tabs)
        {
            Button btn = tabButtonDictList[index];
            btn.onClick.RemoveAllListeners();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = tab.ToString();

            btn.onClick.AddListener(() => OnTabButtonClick(tab));
            index++;
        }
    }

    /// <summary>
    /// 탭 클릭 시 동작
    /// </summary>
    protected virtual void OnTabButtonClick(Parts category)
    {
        currentCategory = category;
        animator.Play("BookNextPage");

        SetTypeButtons(category);
        SetItemButtons(category, GetDefaultType());
    }

    /// <summary>
    /// 타입 버튼 초기화
    /// </summary>
    protected virtual void SetTypeButtons(Parts tab)
    {
        var types = GetUsableTypes();
        int index = 0;

        foreach (T type in types)
        {
            Debug.Log(index);
            Debug.Log($"타입버튼딕트 리스트의 길이 {typeButtonDictList.Count}");
            Button btn = typeButtonDictList[index];
            btn.onClick.RemoveAllListeners();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = type.ToString();
            btn.onClick.AddListener(() => SetItemButtons(tab, type));
            index++;
        }
    }

    /// <summary>
    /// 아이템 버튼 초기화
    /// </summary>
    protected virtual void SetItemButtons(Parts category, T type)
    {
        var items = itemDict[category][type];

        for (int i = 0; i < DICTSIZE; i++)
        {
            ItemData itemData = items[i];
            Button button = itemButtonDictList[i];

            button.GetComponent<ItemButton>().itemData = itemData;
            button.GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;

            if (i < itemButtonWrappers.Count)
                itemButtonWrappers[i].itemData = itemData;
            else
                itemButtonWrappers.Add(new ButtonWrapper("아이템버튼", button, itemData));

            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnItemButtonClick(itemButtonWrappers[index].itemData));
        }
    }

    /// <summary>
    /// 아이템 버튼 클릭 시
    /// </summary>
    protected virtual void OnItemButtonClick(ItemData itemData)
    {
        prevItemImage.sprite = itemData.ItemImage;

        prevTextDictList["PrevName"].text = itemData.Name;
        prevTextDictList["ATK"].text = itemData.Attack.ToString();
        prevTextDictList["DEF"].text = itemData.Defence.ToString();
        prevTextDictList["HP"].text = itemData.Hp.ToString();

        prevTextDictList["Ingrediant1"].text = itemData.Name;
        prevTextDictList["Ingrediant2"].text = itemData.Name;
        prevTextDictList["Ingrediant3"].text = itemData.Name;
        prevTextDictList["Ingrediant4"].text = itemData.Name;

        createButton.curSelectedItem = itemData;
    }
}