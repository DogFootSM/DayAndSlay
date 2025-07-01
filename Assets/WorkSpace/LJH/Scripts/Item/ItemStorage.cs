using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : MonoBehaviour
{
    [Header("아이템")]
    public List<Item> ItemList = new List<Item>();

    [Header("아이템 데이터")]
    public List<ItemData> ItemDatas = new List<ItemData>();
    public Dictionary<int, ItemData> ItemDatasDict = new Dictionary<int, ItemData>();

    [Header("재료 아이템")]
    public List<ItemData> Ingrediants = new List<ItemData>();
    public Dictionary<int, ItemData> IngrediantDict = new Dictionary<int, ItemData>();
    
    [Header("아이템 레시피")]
    public List<ItemRecipe> ItemRecipes = new List<ItemRecipe>();


    private void Start()
    {
        MakeItemDataDict();
    }

    private void MakeItemDataDict()
    {
        for (int i = 0; i < ItemDatas.Count; i++)
        {
            ItemDatasDict[ItemDatas[i].ItemId] = ItemDatas[i];
        }

        for (int i =0; i < Ingrediants.Count; i++)
        {
            IngrediantDict[Ingrediants[i].ItemId] = Ingrediants[i];
        }
    }
    public ItemData GetItemById(Dictionary<int, ItemData> dictionary, int id)
    {
        //아이템 아이디를 인수로 넣으면
        //아이템 목록에서 그 아이디를 가진 아이템을 반환해줘야함

        return dictionary[id];
    }
}
