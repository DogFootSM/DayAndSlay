using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryInteraction : 
    InventoryController, 
    IPointerUpHandler,IPointerDownHandler,IPointerClickHandler,
    IDragHandler
{ 
    [SerializeField] GraphicRaycaster inventoryRayCanvas;
    [SerializeField] private Image dragItemImage;
    [SerializeField] private Image detailItemImage;
    [SerializeField] private TextMeshProUGUI detailItemDescA;
    [SerializeField] private TextMeshProUGUI detailItemDescB;
    
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
         
        //마우스가 이동하는 위치로 아이템 이미지 위치 변경
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
        inventoryRayCanvas.Raycast(eventData, results);
         
        if (results.Count < 2) return;
        
        //이동 슬롯
        InventorySlot compareSlot = results[1].gameObject.GetComponentInParent<InventorySlot>();

        if (compareSlot != null && compareSlot.CurSlotItem == null)
        {
            //이동 슬롯에 아이템이 없을 경우 새 슬롯 아이템 이동 및 기존 슬롯 삭제
            compareSlot.AddItem(detectedInventorySlot.CurSlotItem, detectedInventorySlot.ItemCount);
            detectedInventorySlot.RemoveItem();
        }
        else if (compareSlot != null && compareSlot.CurSlotItem != null)
        {
            Item temp = detectedInventorySlot.CurSlotItem;
            int tempCount = detectedInventorySlot.ItemCount;
            
            //이동 슬롯과 기존 슬롯의 내용 교환
            detectedInventorySlot.ChangeItem(compareSlot.CurSlotItem, compareSlot.ItemCount);
            compareSlot.ChangeItem(temp, tempCount);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        results.Clear();
        
        inventoryRayCanvas.Raycast(eventData, results);

        detectedInventorySlot = results[0].gameObject.GetComponentInParent<InventorySlot>(); 
        SetSlotItemDetail();
        
        if (DetectedInventorySlotItem()) return;

        detailItemImage.sprite = detectedInventorySlot.CurSlotItem.itemData.ItemImage;
        detailItemDescA.text = detectedInventorySlot.CurSlotItem.itemData.ItemDescA;
        detailItemDescB.text = detectedInventorySlot.CurSlotItem.itemData.ItemDescB; 
    }
    
    /// <summary>
    /// 인벤토리 클릭 동작
    /// </summary>
    private void OnInventorySlotClick()
    {
        //드래그에 사용할 이미지 위치 슬롯 아이템 이미지 위치로 변경
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

    /// <summary>
    /// 아이템 상세 UI 설정
    /// </summary>
    private void SetSlotItemDetail()
    {
        detailItemImage.gameObject.SetActive(!DetectedInventorySlotItem());
        detailItemDescA.gameObject.SetActive(!DetectedInventorySlotItem());
        detailItemDescB.gameObject.SetActive(!DetectedInventorySlotItem());
    }
    
}
