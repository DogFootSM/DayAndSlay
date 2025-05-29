using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryInteraction : InventoryController, IPointerUpHandler,IPointerDownHandler, IDragHandler
{ 
    [SerializeField] GraphicRaycaster inventoryRayCanvas;
    [SerializeField] private Image dragItemImage;
    
    private HashSet<int> ownedItemSet = new HashSet<int>();
    private List<RaycastResult> results = new List<RaycastResult>();
    
    private InventorySlot detectedInventorySlot;
    private bool DetectedInventorySlotItem() => detectedInventorySlot.CurSlotItem == null;
    
    /// <summary>
    /// 아이템 습득 후 인벤토리에 저장
    /// </summary>
    /// <param name="collectedItem"></param>
    public void AddItemToInventory(Item collectedItem)
    {
        if (ownedItemSet.Contains(collectedItem.itemData.ItemId)
            && collectedItem.itemData.IsOverlaped)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].CurSlotItem != null
                    && inventorySlots[i].CurSlotItem.itemData.IsOverlaped
                    && inventorySlots[i].CurSlotItem.itemData.ItemId == collectedItem.itemData.ItemId)
                {
                    inventorySlots[i].AddItem(collectedItem); 
                    //TODO: 아이템 습득 애니메이션 재생
                    return;
                }            
            }
        }
 
        var emptySlots = inventorySlots.FirstOrDefault(x => x.CurSlotItem == null);
        
        if (emptySlots != null)
        {
            emptySlots.AddItem(collectedItem);
            ownedItemSet.Add(collectedItem.itemData.ItemId);
            //TODO: 아이템 습득 애니메이션 재생
            return;
        }
        
        Debug.Log("빈 슬롯이 없습니다.");
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {  
        if (DetectedInventorySlotItem()) return;
         
        dragItemImage.transform.position = eventData.position;
    }

    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        results.Clear();
        inventoryRayCanvas.Raycast(eventData, results);
        detectedInventorySlot = results[0].gameObject.GetComponentInParent<InventorySlot>();

        if (DetectedInventorySlotItem()) return; 
        
        OnInventorySlotClick();

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (DetectedInventorySlotItem()) return;

        OnInventorySlotMouseUp();
        //인벤토리 슬롯 교환 진행
        
    }

    /// <summary>
    /// 인벤토리 클릭 동작
    /// </summary>
    private void OnInventorySlotClick()
    {
        dragItemImage.transform.position = detectedInventorySlot.transform.position; 
        dragItemImage.gameObject.SetActive(true);
        dragItemImage.sprite = detectedInventorySlot.CurSlotItem.itemData.ItemImage;
        detectedInventorySlot.OnDragScaleEvent?.Invoke();
    }

    /// <summary>
    /// 아이템 클릭 후 마우스 뗐을 때 동작
    /// </summary>
    private void OnInventorySlotMouseUp()
    {
        dragItemImage.gameObject.SetActive(false);
        detectedInventorySlot.OnResetScaleEvent?.Invoke();
    }
    
}
