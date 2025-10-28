using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
    public List<int> ids;
    public List<Sprite> sprites;

    //최적화를 위해 Dictionary로 변환하여 관리
    [SerializeField][SerializedDictionary] SerializedDictionary<int, ItemData> itemIdToDataDict;
    [SerializeField][SerializedDictionary] SerializedDictionary<string, int> itemNameToIdDict;

    [SerializeField][SerializedDictionary] SerializedDictionary<int, string> recipeIdToNameDict;

    private void OnEnable()
    {
        InitDict();
    }
    private void InitDict()
    {
        itemIdToDataDict = new SerializedDictionary<int, ItemData>();
        itemNameToIdDict = new SerializedDictionary<string, int>();

        recipeIdToNameDict = new SerializedDictionary<int, string>();

        foreach (ItemData item in items)
        {
            itemIdToDataDict[item.ItemId] = item;
            recipeIdToNameDict[item.ItemId] = item.Name;
        }

        if (ids.Count != items.Count)
        {
            ids.Clear();
            foreach (ItemData item in items)
            {
                ids.Add(item.ItemId);
            }
        }

        foreach (int id in ids)
        {
            itemNameToIdDict[itemIdToDataDict[id].Name] = itemIdToDataDict[id].ItemId;
        }


        foreach (ItemData item in items)
        {
            item.ItemImageId =  item.ItemId;
            item.ItemImage = GetItemImage(item.ItemImageId);
        }


        
    }

    private Sprite GetItemImage(int id)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == id.ToString())
            {
                return sprite;
            }
        }
        
        return null;
    }

    public ItemData GetItemByID(int id)
    {
        if (itemIdToDataDict.TryGetValue(id, out ItemData item))
            return item;

        return null;
    }

    public int GetItemByName(string name)
    {
        if (itemNameToIdDict.TryGetValue(name, out int itemId))
            return itemId;

        return 0;
    }    

}
