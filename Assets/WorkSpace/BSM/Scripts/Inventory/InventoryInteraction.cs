using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInteraction : InventoryController, IPointerEnterHandler, IPointerClickHandler, IPointerMoveHandler
{
    [SerializeField] private Item tempItem;
    
    
    private void Update()
    {
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            inventorySlots[0].AddItem(tempItem);
        }
        
        
    }

    public void AddItemToInventory(Item collectedItem)
    { 
        //리스트 내에서 아이템을 소유하지 않은 슬롯 확인
            //해당 슬롯에 아이템 추가
            
        //빈 슬롯이 없을 경우
        
        Debug.Log("빈 슬롯이 없습니다.");
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerMove(PointerEventData eventData)
    {
         
    }
}
