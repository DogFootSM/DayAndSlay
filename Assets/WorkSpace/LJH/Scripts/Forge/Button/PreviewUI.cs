using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAtk;
    [SerializeField] private TextMeshProUGUI itemDef;
    [SerializeField] private TextMeshProUGUI itemHp;
    [SerializeField] private TextMeshProUGUI itemIngredient1;
    [SerializeField] private TextMeshProUGUI itemIngredient2;
    [SerializeField] private TextMeshProUGUI itemIngredient3;
    [SerializeField] private TextMeshProUGUI itemIngredient4;
    
    private ItemDatabaseManager itemDatabaseManager;

    private void Start()
    {
        Init();
        
        itemDatabaseManager = ItemDatabaseManager.instance;
    }
    public void SetPreview(ItemData itemData)
    {
        itemName.text =  itemData.Name;
        itemImage.sprite = itemData.ItemImage;
        itemAtk.text = itemData.Attack.ToString();
        itemDef.text = itemData.Defence.ToString();
        itemHp.text = itemData.Hp.ToString();
        
        SetIngredientTextColor(itemIngredient1, itemDatabaseManager.GetItemByID(itemData.ingredients_1));
        SetIngredientTextColor(itemIngredient2, itemDatabaseManager.GetItemByID(itemData.ingredients_2));
        SetIngredientTextColor(itemIngredient3, itemDatabaseManager.GetItemByID(itemData.ingredients_3));

        if (itemData.ingredients_4 != 000000)
        {
            itemIngredient4.transform.parent.gameObject.SetActive(true);
            SetIngredientTextColor(itemIngredient4, itemDatabaseManager.GetItemByID(itemData.ingredients_4));
        }
        else
        {
            itemIngredient4.transform.parent.gameObject.SetActive(false);
        }
    }

    private void SetIngredientTextColor(TextMeshProUGUI text, ItemData itemData)
    {
        //B4A721 희귀 재료 색
        //6F3232 기본 재료 색
        Color color = text.color;
        
        if (itemData.Parts == Parts.RARE_INGREDIANT)
        {
            ColorUtility.TryParseHtmlString("#B4A721", out color);
        }

        else if (itemData.Parts == Parts.INGREDIANT)
        {
            ColorUtility.TryParseHtmlString("#6F3232", out color);
        }

        else
        {
            Debug.LogWarning("재료 아이템이 아닙니다.");
        }
        text.color = color;
        text.text = itemData.Name;
    }





    private void Init()
    {
        itemName = GetUI<TextMeshProUGUI>("ItemName");
        itemImage = GetUI<Image>("ItemImage");
        itemAtk = GetUI<TextMeshProUGUI>("ATK");
        itemDef = GetUI<TextMeshProUGUI>("DEF");
        itemHp = GetUI<TextMeshProUGUI>("HP");
        
        itemIngredient1 = GetUI<TextMeshProUGUI>("Ingredient1");
        itemIngredient2 = GetUI<TextMeshProUGUI>("Ingredient2");
        itemIngredient3 = GetUI<TextMeshProUGUI>("Ingredient3");
        itemIngredient4 = GetUI<TextMeshProUGUI>("Ingredient4");
    }

}
