using AYellowpaper.SerializedCollections;
using AYellowpaper.SerializedCollections.Editor.Search;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class BaseForgeUI : BaseUI
{
    //버튼의 이름 보여주고 기능 처리만함
    [Inject] TestPlayer player;

    [SerializeField] protected Parts parts;

    [SerializeField][SerializedDictionary] private SerializedDictionary<string, TextMeshProUGUI> prevTextDict;

    [SerializeField] Button createButton;

    protected List<Button> typeButtonList = new List<Button>();
    protected List<Button> itemButtonList = new List<Button>();
    protected List<GameObject> hideList = new List<GameObject>();

    protected ItemData curItem;

    protected int defaultNum = 0;
    protected abstract void ButtonInit();
    protected abstract void SetTypeButton(Parts parts);
    protected abstract void SetItemButton(int typeIndex);
    
    private void Start()
    {
        Init();
        ButtonInit();
    }

    private void ButtonActivate()
    {
        foreach (Button btn in itemButtonList)
        {
            ItemButton itemButton = btn.GetComponent<ItemButton>();
            if (itemButton == null)
            {
                btn.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < itemButtonList.Count; i++)
        {
            if (itemButtonList[i].gameObject.activeSelf)
            {
                hideList[i].gameObject.SetActive(itemButtonList[i].gameObject.activeSelf);
            }
        }

        ItemDatabaseManager.instance.GetItemByID(0000000);
    }

    private void UpdateHideList()
    {
        foreach (Button btn in itemButtonList)
        {
            if(!btn.GetComponent<ItemButton>())
            {
                return;
            }

            ItemRecipe itemRecipe = btn.GetComponent<ItemButton>().itemRecipe;
            if(player.inventories.Contains(ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_1)))
            {
                hideList[itemButtonList.IndexOf(btn)].SetActive(false);
            }
            else
            {
                hideList[itemButtonList.IndexOf(btn)].SetActive(true);
            }
        }
    }
    protected virtual void Tap_TabButton(Parts parts)
    {
        /*
         타입버튼 디폴트로 눌린거 넣어주면 됨
        */
        SetTypeButton(parts);
        Tap_TypeButton(defaultNum);
    }

    protected void Tap_TypeButton(int typeIndex)
    {
        /*
         아이템 버튼 디폴트로 눌린거 보여주면됨
        */
        SetItemButton(typeIndex);

        //아이템에 맞는 레시피 세팅
        foreach (Button btn in itemButtonList)
        {
            if (btn.GetComponent<ItemButton>())
            {
                ItemButton itemBtn = btn.GetComponent<ItemButton>();
                itemBtn.itemRecipe = Set_ItemRecipe(itemBtn.itemData.ItemId);
            }
        }


        Tap_ItemButton(defaultNum);
    }

    protected void Tap_ItemButton(int index)
    {
        ButtonActivate();
        //Debug.Log("아이템 버튼 선택해서 낡은 검이 미리보기에 보임");
        /*
         미리보기에 지금 누른 아이템 넣어줌
        */
        UpdateHideList();
        ItemButton btn = itemButtonList[index].GetComponent<ItemButton>();
        ItemData itemData = btn.itemData;
        ItemRecipe itemRecipe = btn.itemRecipe;

        prevTextDict["name"].text = itemData.Name;
        prevTextDict["atk"].text = itemData.Attack.ToString();
        prevTextDict["def"].text = itemData.Defence.ToString();
        prevTextDict["hp"].text = itemData.Hp.ToString();


        prevTextDict["ingre1"].text = ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_1).Name.ToString();
        prevTextDict["ingre2"].text = ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_2).Name.ToString();
        prevTextDict["ingre3"].text = ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_3).Name.ToString();
        prevTextDict["ingre4"].text = "";

        if (ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_4) != null)
        {
            prevTextDict["ingre4"].text = ItemDatabaseManager.instance.GetItemByID(itemRecipe.ingredients_4).Name.ToString();
        }

        curItem = itemData;
    }

    /// <summary>
    /// Setting ItemRecipe in ItemButtons.
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    private ItemRecipe Set_ItemRecipe(int itemId)
    {
        return ItemDatabaseManager.instance.GetRecipeByID(itemId);
    }

    public void CreateItem()
    {
        if(curItem == null)
        {
            Debug.Log($"선택된 아이템이 없습니다. 제작할 아이템을 선택해 주세요.");
            return;
        }
        Debug.Log($"{curItem}을 생성하였습니다.");
        player.inventories.Add(curItem);
    }

    private void Init()
    {
        prevTextDict.Add("name", GetUI<TextMeshProUGUI>("ItemName"));
        prevTextDict.Add("atk", GetUI<TextMeshProUGUI>("ATK"));
        prevTextDict.Add("def", GetUI<TextMeshProUGUI>("DEF"));
        prevTextDict.Add("hp", GetUI<TextMeshProUGUI>("HP"));

        prevTextDict.Add("ingre1", GetUI<TextMeshProUGUI>("Ingrediant1"));
        prevTextDict.Add("ingre2", GetUI<TextMeshProUGUI>("Ingrediant2"));
        prevTextDict.Add("ingre3", GetUI<TextMeshProUGUI>("Ingrediant3"));
        prevTextDict.Add("ingre4", GetUI<TextMeshProUGUI>("Ingrediant4"));

        createButton.onClick.AddListener(() => CreateItem());

    }

}