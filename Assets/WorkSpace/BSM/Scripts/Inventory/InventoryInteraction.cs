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
    [SerializeField] private Button equipButton;
  
    private HashSet<int> ownedItemSet = new HashSet<int>();
    private List<RaycastResult> results = new List<RaycastResult>();
    
    private InventorySlot detectedInventorySlot;
    private bool DetectedInventorySlotItem() => detectedInventorySlot.CurSlotItem == null;
    
    new void Awake()
    {
        base.Awake();
        //SetSlotItemData();
        //SetOwnedItemSet(); 
        //equipButton.onClick.AddListener(Equip);
    }

    private void Start()
    {
        //TODO: 테스트용으로 Start에서 호출
        SetSlotItemData();
        SetOwnedItemSet(); 
        equipButton.onClick.AddListener(Equip);
    }

    private void Equip()
    { 
        //TODO: 아이템 장착 시 Equipment로 해당 아이템을 넘겨주고
    }
    
    /// <summary>
    /// 아이템 ID HashSet 설정
    /// </summary>
    private void SetOwnedItemSet()
    {
        for (int i = 0; i < GetItemId().Count; i++)
        {
            ownedItemSet.Add(GetItemId()[i].itemId); 
        } 
    }
    
    /// <summary>
    /// 아이템 습득 후 인벤토리에 저장
    /// </summary>
    /// <param name="collectedItem">습득 아이템 객체</param>
    public void AddItemToInventory(ItemData collectedItem)
    { 
        if (ownedItemSet.Contains(collectedItem.ItemId)
            && collectedItem.IsOverlaped)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                //아이템 id가 같은것을 슬롯에서 찾으면 아이템 슬롯에 추가
                if (inventorySlots[i].CurSlotItem.ItemId == collectedItem.ItemId)
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
            ownedItemSet.Add(collectedItem.ItemId);
            //TODO: 아이템 습득 애니메이션 재생
            return;
        }
        
        //TODO: 빈 슬롯에 대한 안내 방법 추가
        Debug.Log("빈 슬롯이 없습니다.");
        
    }
    
    /// <summary>
    /// 아이템 슬롯 드래그 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {  
        if (DetectedInventorySlotItem()) return;
         
        //마우스가 이동하는 위치로 아이템 이미지 위치 변경
        dragItemImage.transform.position = eventData.position;
    }

    /// <summary>
    /// 아이템 슬롯 마우스 눌렀을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        results.Clear();
        inventoryRayCanvas.Raycast(eventData, results);
        detectedInventorySlot = results[0].gameObject.GetComponentInParent<InventorySlot>();

        if (DetectedInventorySlotItem()) return; 
        
        OnInventorySlotClick();

    }

    /// <summary>
    /// 아이템 슬롯 마우스 뗐을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
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
            //장비 착용 여부 교환
            compareSlot.IsEquipItem = detectedInventorySlot.IsEquipItem;
            detectedInventorySlot.IsEquipItem = false;
            
            //이동 슬롯에 아이템이 없을 경우 새 슬롯 아이템 이동 및 기존 슬롯 삭제
            compareSlot.AddItem(detectedInventorySlot.CurSlotItem, detectedInventorySlot.ItemCount);
            detectedInventorySlot.RemoveItem();
        }
        else if (compareSlot != null && compareSlot.CurSlotItem != null)
        {
            ItemData temp = detectedInventorySlot.CurSlotItem;
            int tempCount = detectedInventorySlot.ItemCount;
            bool tempIsEquip = detectedInventorySlot.IsEquipItem;
            
            //이동 슬롯과 기존 슬롯의 내용 교환
            detectedInventorySlot.ChangeItem(compareSlot.CurSlotItem, compareSlot.ItemCount, compareSlot.IsEquipItem);
            compareSlot.ChangeItem(temp, tempCount, tempIsEquip);
        }
        
    }
    
    /// <summary>
    /// 아이템 슬롯 클릭했을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        results.Clear();
        
        inventoryRayCanvas.Raycast(eventData, results);

        detectedInventorySlot = results[0].gameObject.GetComponentInParent<InventorySlot>(); 
        SetSlotItemDetail();
        
        if (DetectedInventorySlotItem()) return;

        detailItemImage.sprite = detectedInventorySlot.CurSlotItem.ItemImage;
        detailItemDescA.text = detectedInventorySlot.CurSlotItem.ItemDescA;
        detailItemDescB.text = detectedInventorySlot.CurSlotItem.ItemDescB;
         
        //사용 가능한 아이템이면 버튼 활성화
        equipButton.gameObject.SetActive(detectedInventorySlot.CurSlotItem.IsEquipment);
        
        //TODO: 버튼 활성화 됐을 때, 이미 착용중인 아이템이면 클릭 불가하게
        
    }
    
    /// <summary>
    /// 인벤토리 클릭 동작
    /// </summary>
    private void OnInventorySlotClick()
    {
        //드래그에 사용할 이미지 위치 슬롯 아이템 이미지 위치로 변경
        dragItemImage.transform.position = detectedInventorySlot.transform.position; 
        dragItemImage.gameObject.SetActive(true);
        dragItemImage.sprite = detectedInventorySlot.CurSlotItem.ItemImage;
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
        //TODO: 추후 오브젝트들 lIST에 담아서 순회하는 방식으로 On,Off 하기
        detailItemImage.gameObject.SetActive(!DetectedInventorySlotItem());
        detailItemDescA.gameObject.SetActive(!DetectedInventorySlotItem());
        detailItemDescB.gameObject.SetActive(!DetectedInventorySlotItem());
    }
    
}
