using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSM_ItemManager
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private List<ItemData> items;
        public static ItemManager instance;

        private Dictionary<int, ItemData> itemDict;
        
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

            Init();
        }

        private void Init()
        {
            itemDict = new Dictionary<int, ItemData>(items.Count << 2);
             
            for (int i = 0; i < items.Count; i++)
            {
                if (!itemDict.ContainsKey(items[i].ItemId))
                {
                    itemDict[items[i].ItemId] = items[i];
                } 
            } 
        }
        
        public ItemData GetItemData(int itemId)
        {
            if (itemDict.TryGetValue(itemId, out ItemData itemData))
            {
                return itemData;
            }

            return null;
        } 
    }
}

