using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    [SerializeField] private InventoryInteraction inventoryInteraction;

    private LayerMask itemLayerMask;

    private void Awake()
    {
        itemLayerMask = LayerMask.GetMask("Item"); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & itemLayerMask) != 0)
        {
            Item grabItem = other.GetComponent<Item>();

            if (grabItem != null)
            { 
                //드랍된 아이템의 ItemData 추가
                inventoryInteraction.AddItemToInventory(grabItem.itemData);   
            } 
        }
    } 
}
