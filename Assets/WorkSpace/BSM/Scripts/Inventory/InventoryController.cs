using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;
using Items = BSM_ItemManager.ItemManager;


public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    
    [Inject] private DataManager dataManager;
    [Inject] private SqlManager sqlManager;
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private IDataReader dataReader;
    
    private Items itemManager => Items.instance;
    
    protected void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
         
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            inventorySlots.Add(slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
         
        //TODO: 아이템 데이터들 각 슬롯에 할당해주기

        dataReader = sqlManager.ReadItemDataColumn("slot_id", $"{dataManager.SlotId}");

        int x = 0;
        
        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                Debug.Log(i);
            }
        }  
        
    }
    
}
