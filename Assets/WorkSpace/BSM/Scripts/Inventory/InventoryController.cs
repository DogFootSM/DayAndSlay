using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();

    protected void Awake()
    {
        for (int i = 0; i < slotParent.transform.childCount; i++)
        {
            inventorySlots.Add(slotParent.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
         
    }
    
}
