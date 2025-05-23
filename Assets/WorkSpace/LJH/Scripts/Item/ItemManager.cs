using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("아이템")]
    public List<Item> ItemList = new List<Item>();
    [Header("아이템 데이터")]
    public List<ItemData> ItemDatas = new List<ItemData>();
    [Header("재료 아이템")]
    public List<Ingredient> Ingredients = new List<Ingredient>();
    [Header("아이템 레시피")]
    public List<ItemRecipe> ItemRecipes = new List<ItemRecipe>();
}
