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
    
    [Inject] protected DataManager dataManager;
    [Inject] protected SqlManager sqlManager;
 
    public static Dictionary<int, InventorySlot> OwnedMaterialDict = new Dictionary<int, InventorySlot>();
    public static event Action OnClosedInventory;
    
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();
    protected Dictionary<Parts, InventorySlot> equipSlotDict = new Dictionary<Parts, InventorySlot>();
    
    private List<BSM_ItemData> itemDatas = new List<BSM_ItemData>();
    private IDataReader dataReader;
    private ItemDatabaseManager itemManager => ItemDatabaseManager.instance;

    protected void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
         
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            inventorySlots.Add(slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        } 
    }

    private void OnDisable()
    { 
        OnClosedInventory?.Invoke();
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
            //제작 재료 아이템을 저장한 슬롯 딕셔너리 저장
            AddMaterialItemToDictionary(itemDatas[i].itemId, inventorySlots[itemDatas[i].inventorySlotId]);
            
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
    /// 제작 재료 아이템을 가지고 있는 슬롯을 딕셔너리에 저장
    /// </summary>
    /// <param name="itemId">제작 재료 아이템 아이디</param>
    /// <param name="slot">아이템을 가지고 있는 인벤토리 슬롯</param>
    protected void AddMaterialItemToDictionary(int itemId, InventorySlot slot)
    {
        if (!itemManager.GetItemByID(itemId).IsEquipment)
        {
            OwnedMaterialDict.TryAdd(itemId, slot);
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
