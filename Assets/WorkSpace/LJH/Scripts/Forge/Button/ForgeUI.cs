using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : BaseUI
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

        itemIngredient1.text = itemDatabaseManager.GetItemByID(itemData.ingredients_1).ToString();
        itemIngredient2.text = itemDatabaseManager.GetItemByID(itemData.ingredients_2).ToString();
        itemIngredient3.text = itemDatabaseManager.GetItemByID(itemData.ingredients_3).ToString();

        if (itemData.ingredients_4 != 000000)
        {
            itemIngredient4.text = itemDatabaseManager.GetItemByID(itemData.ingredients_4).ToString();
        }
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
