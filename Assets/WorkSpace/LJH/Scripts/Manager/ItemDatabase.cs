using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    //최적화를 위해 Dictionary로 변환하여 관리
    [SerializeField][SerializedDictionary] SerializedDictionary<int, ItemData> itemDict;

    private void OnEnable()
    {
        InitDict();
    }
    private void InitDict()
    {
        itemDict = new SerializedDictionary<int, ItemData>();
        foreach (ItemData item in items)
        {
            itemDict[item.ItemId] = item;
        }
    }

    public ItemData GetItemByID(int id)
    {
        if (itemDict.TryGetValue(id, out ItemData item))
            return item;

        Debug.LogWarning($"ID {id} 에 해당하는 아이템이 없습니다.");
        return null;
    }
}
