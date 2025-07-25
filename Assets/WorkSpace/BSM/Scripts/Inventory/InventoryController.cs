using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;
using BSM_ItemData = BSM_ITEM.ItemData;

namespace BSM_ITEM
{
    /// <summary>
    /// DB에서 읽어온 아이템 데이터 저장
    /// </summary>
    public struct ItemData
    {
        public int itemId;
        public int itemAmount;
        public int inventorySlotId;
        public bool isEquipment;
    }
}
 
public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    [SerializeField] protected Equipment equipment;
    
    [Inject] private DataManager dataManager;
    [Inject] protected SqlManager sqlManager;
 
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<BSM_ItemData> itemDatas = new List<BSM_ItemData>();
    private IDataReader dataReader;
    protected Dictionary<Parts, InventorySlot> equipSlotDict = new Dictionary<Parts, InventorySlot>();

    private ItemDatabaseManager itemManager => ItemDatabaseManager.instance;
    
    protected void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
         
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            inventorySlots.Add(slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
          
    }

    /// <summary>
    /// 아이템 데이터 인벤토리 슬롯에 할당
    /// </summary>
    protected void SetSlotItemData()
    { 
        dataReader = sqlManager.ReadItemDataColumn("slot_id", $"{dataManager.SlotId}");
 
        while (dataReader.Read())
        {
            BSM_ItemData data = new BSM_ItemData()
            {
                itemId = dataReader.GetInt32(0),
                itemAmount = dataReader.GetInt32(2),
                inventorySlotId = dataReader.GetInt32(3),
                isEquipment = dataReader.GetBoolean(4),
            };
                
            itemDatas.Add(data);
        }

        for (int i = 0; i < itemDatas.Count; i++)
        {  
            Debug.Log($"data:{itemManager.GetItemByID(itemDatas[i].itemId) == null}, manager:{itemManager == null}");
            inventorySlots[itemDatas[i].inventorySlotId].AddItem(
                itemManager.GetItemByID(itemDatas[i].itemId),
                itemDatas[i].itemAmount);
            
            //장비 착용 여부 설정
            inventorySlots[itemDatas[i].inventorySlotId].IsEquip = itemDatas[i].isEquipment;
            
            //장착중이었던 장비인지 확인
            if (inventorySlots[itemDatas[i].inventorySlotId].IsEquip)
            {
                //DB에서 받아온 아이템 정보로 착용 장비 설정
                equipment.EquipItem(inventorySlots[itemDatas[i].inventorySlotId].CurSlotItem);
                 
                if (inventorySlots[itemDatas[i].inventorySlotId].CurSlotItem.IsEquipment)
                {
                    equipSlotDict[inventorySlots[itemDatas[i].inventorySlotId].CurSlotItem.Parts] = inventorySlots[itemDatas[i].inventorySlotId];
                } 
            } 
        } 
    }

    /// <summary>
    /// 아이템 데이터 반환
    /// </summary>
    /// <returns>현재 보유중인 아이템 데이터</returns>
    protected List<BSM_ItemData> GetItemId()
    { 
        return itemDatas;
    }
}
