using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
            //이재호가 추가한 헬스팩 먹는 로직
            if (other.GetComponent<HealthPack>())
            {
                HealthPack healthPack = other.GetComponent<HealthPack>();
                
                healthPack.StartPickupEffect(transform);
                GetComponentInParent<PlayerController>().PlayerHealing(healthPack.HpRecoveryAmount);

                return;
            }
            
            
            
            Item grabItem = other.GetComponent<Item>();

            if (grabItem != null)
            { 
                //드랍된 아이템의 ItemData 추가
                
                //1 ~5 개 아이템 습득
                int random = Random.Range(1, 6);
                bool isEat = true;

                isEat = inventoryInteraction.AddItemToInventory(grabItem.itemData, random);

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
