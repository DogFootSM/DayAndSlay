using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrab : MonoBehaviour
{
    [SerializeField] private InventoryInteraction inventoryInteraction;
    [SerializeField] private Animator notEmptySlotAnimator;
    
    private LayerMask itemLayerMask;
    private int emptySlotHash = Animator.StringToHash("EmptySlot");
    
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
                bool isEat = inventoryInteraction.AddItemToInventory(grabItem.itemData);

                if (isEat)
                {
                    grabItem.StartPickupEffect(transform);
                }
                else
                {
                    notEmptySlotAnimator.Play(emptySlotHash);
                }
            } 
        }
    } 
}
