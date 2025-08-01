using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseManager : MonoBehaviour
{
    public static ItemDatabaseManager instance;
    public ItemDatabase ItemDatabase { get => itemDatabase; }
    
    [SerializeField] private ItemDatabase itemDatabase;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemData GetItemByID(int ID) => itemDatabase.GetItemByID(ID);

    public ItemData GetItemByName(string name) => GetItemByID(itemDatabase.GetItemByName(name));

    /// <summary>
    /// 장비 아이템 한번에 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetAllEquipItem()
    {
        List<ItemData> list = new List<ItemData>();
        
        foreach (ItemData item in itemDatabase.items)
        {
            if (item.IsEquipment)
            {
                list.Add(item);        
            }
        }
        
        return list;
    }
    
    /// <summary>
    /// 재료 아이템 한번에 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ItemData> GetAllIngrediantItem()
    {
        List<ItemData> list = new List<ItemData>();
        
        foreach (ItemData item in itemDatabase.items)
        {
            if (!item.IsEquipment)
            {
                list.Add(item);        
            }
        }
        
        return list;
    }

}