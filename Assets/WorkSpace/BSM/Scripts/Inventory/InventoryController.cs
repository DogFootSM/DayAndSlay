using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    
    [Inject] private DataManager dataManager;
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();

    protected void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            inventorySlots.Add(slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
         
        //TODO: 아이템 데이터들 각 슬롯에 할당해주기
    }
    
}
