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

    //최적화를 위해 Dictionary로 변환하여 관리
    [SerializeField][SerializedDictionary] SerializedDictionary<int, ItemData> itemIdToDataDict;
    [SerializeField][SerializedDictionary] SerializedDictionary<string, int> itemNameToIdDict;

    private void OnEnable()
    {
        InitDict();
    }
    private void InitDict()
    {
        itemIdToDataDict = new SerializedDictionary<int, ItemData>();
        foreach (ItemData item in items)
        {
            itemIdToDataDict[item.ItemId] = item;

            if (ids.Count < items.Count)
            {
                ids.Clear();
                ids.Add(item.ItemId);
            }
        }

        itemNameToIdDict = new SerializedDictionary<string, int>();
        foreach (int id in ids)
        {
            itemNameToIdDict[itemIdToDataDict[id].Name] = itemIdToDataDict[id].ItemId;
        }
    }

    public ItemData GetItemByID(int id)
    {
        if (itemIdToDataDict.TryGetValue(id, out ItemData item))
            return item;

        Debug.LogWarning($"ID {id} 에 해당하는 아이템이 없습니다.");
        return null;
    }

    public int GetItemByName(string name)
    {
        if (itemNameToIdDict.TryGetValue(name, out int itemId))
            return itemId;

        Debug.LogWarning($"아이템명 {name} 에 해당하는 아이템이 없습니다.");
        return 0;
    }    
}
