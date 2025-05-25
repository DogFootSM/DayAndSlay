using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject slotParent;
    
    protected List<InventorySlot> inventorySlots = new List<InventorySlot>();

    protected void Awake()
    {
        GameObject slotInstance = Instantiate(slotParent, transform.GetChild(0).transform);
        
        for (int i = 0; i < slotInstance.transform.childCount; i++)
        {
            inventorySlots.Add(slotInstance.transform.GetChild(i).GetComponentInChildren<InventorySlot>());
        }
         
    }
    
}
