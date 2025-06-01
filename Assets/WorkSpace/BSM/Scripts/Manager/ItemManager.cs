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
        private List<Item> itemPools = new List<Item>();
        
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
             
            //아이템 데이터 요소 아이템 데이터 딕셔너리에 추가
            for (int i = 0; i < items.Count; i++)
            {
                if (!itemDict.ContainsKey(items[i].ItemId))
                {
                    itemDict[items[i].ItemId] = items[i];
                } 
            }
            
            //아이템 풀에 아이템들 추가
            itemPools = transform.GetComponentsInChildren<Item>().ToList(); 
        }
        
        /// <summary>
        /// 아이템 클래스에서 요청한 ID의 아이템 데이터 반환
        /// </summary>
        /// <param name="itemId">아이템 데이터 id</param>
        /// <returns>아이템 데이터</returns>
        public ItemData GetItemData(int itemId)
        {
            if (itemDict.TryGetValue(itemId, out ItemData itemData))
            {
                return itemData;
            }

            return null;
        }
        
        /// <summary>
        /// 요청한 아이템 반환
        /// </summary>
        /// <param name="itemId">아이템 id</param>
        /// <returns>아이템 객체</returns>
        public Item GetPoolItem(int itemId)
        {
            return itemPools.Where(x => x.ItemId == itemId).FirstOrDefault(); 
        }
        
    }
}

