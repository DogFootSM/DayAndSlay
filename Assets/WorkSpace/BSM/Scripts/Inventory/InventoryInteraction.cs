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
    [SerializeField] private TextMeshProUGUI equipStateButtonText;
    
    private HashSet<int> ownedItemSet = new HashSet<int>();
    private List<RaycastResult> results = new List<RaycastResult>();
    
    private InventorySlot fromSlot;
    private bool fromSlotItem => fromSlot.CurSlotItem == null;
    
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

    /// <summary>
    /// 인벤토리 슬롯의 장비 아이템 장착, 장착 해제
    /// </summary>
    private void Equip()
    {
        if (fromSlot.IsEquip)
        {
            equipment.UnEquipItem(fromSlot.CurSlotItem.Parts);
        }
        else
        {
            equipment.EquipItem(fromSlot.CurSlotItem, fromSlot);
        }
         
        SetEquipStateButtonText(fromSlot.IsEquip);
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
                if(inventorySlots[i].CurSlotItem == null) continue;
                
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
        if (fromSlotItem) return;
         
        //마우스가 이동하는 위치로 아이템 이미지 위치 변경
        dragItemImage.transform.position = eventData.position;
    }

    /// <summary>
    /// 아이템 슬롯 마우스 뗐을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (fromSlotItem) return;

        OnInventorySlotMouseUp();
        //인벤토리 슬롯 교환 진행
        inventoryRayCanvas.Raycast(eventData, results);
         
        if (results.Count < 2) return;
        
        //이동 슬롯
        InventorySlot toSlot = results[1].gameObject.GetComponentInParent<InventorySlot>();

        HandleSlotSwap(toSlot);
        
        //이동 슬롯의 아이템이 장착 상태
        if (toSlot.IsEquip)
        {
            ApplyEquipState(toSlot);
        }
        
        //이전 슬롯이 아이템을 갖고 있고, 장착 상태
        if (fromSlot.CurSlotItem != null && fromSlot.IsEquip)
        {
            ApplyEquipState(fromSlot);  
        }
    }

    /// <summary>
    /// 인벤토리 슬롯 내 아이템 교환
    /// </summary>
    /// <param name="toSlot">2번째로 감지된 슬롯</param>
    private void HandleSlotSwap(InventorySlot toSlot)
    {
        if (toSlot != null && toSlot.CurSlotItem == null)
        {
            //장비 착용 여부 교환
            toSlot.IsEquip = fromSlot.IsEquip;
            fromSlot.IsEquip = false;
            
            //이동 슬롯에 아이템이 없을 경우 새 슬롯 아이템 이동 및 기존 슬롯 삭제
            toSlot.AddItem(fromSlot.CurSlotItem, fromSlot.ItemCount);
            fromSlot.RemoveItem();
        }
        else if (toSlot != null && toSlot.CurSlotItem != null)
        {
            ItemData netData = fromSlot.CurSlotItem;
            int netItemCount = fromSlot.ItemCount;
            bool newEquip = fromSlot.IsEquip;
            
            fromSlot.IsEquip = toSlot.IsEquip;
            toSlot.IsEquip = newEquip;
            
            //이동 슬롯과 기존 슬롯의 내용 교환
            fromSlot.ChangeItem(toSlot.CurSlotItem, toSlot.ItemCount);
            toSlot.ChangeItem(netData, netItemCount);
        }
    }

    /// <summary>
    /// 교환 슬롯의 장착 아이템 변경
    /// </summary>
    /// <param name="toSlot">장착 아이템을 확인할 슬롯</param>
    private void ApplyEquipState(InventorySlot toSlot)
    {  
        equipment.EquipItem(toSlot.CurSlotItem, toSlot);
    }
     
    /// <summary>
    /// 아이템 슬롯 마우스 눌렀을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        results.Clear();
        inventoryRayCanvas.Raycast(eventData, results);
        fromSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
        
        if (fromSlotItem) return; 
        
        OnInventorySlotClick();

    }

    
    /// <summary>
    /// 아이템 슬롯 클릭했을 때 동작
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        results.Clear();
        
        inventoryRayCanvas.Raycast(eventData, results);

        fromSlot = results[0].gameObject.GetComponentInParent<InventorySlot>(); 
        SetSlotItemDetail();
        SetEquipStateButtonText(fromSlot.IsEquip);
        
        if (fromSlotItem) return; 
        detailItemImage.sprite = fromSlot.CurSlotItem.ItemImage;
        detailItemDescA.text = fromSlot.CurSlotItem.ItemDescA;
        detailItemDescB.text = fromSlot.CurSlotItem.ItemDescB; 
    }

    /// <summary>
    /// 아이템 장착 상태에 따른 장착 버튼 텍스트 변경
    /// </summary>
    /// <param name="isEquip">현재 슬롯이 보유중인 아이템의 장착 상태</param>
    private void SetEquipStateButtonText(bool isEquip)
    {
        equipStateButtonText.text = isEquip ? "장착 해제" : "장착";
    }
    
    
    /// <summary>
    /// 인벤토리 클릭 동작
    /// </summary>
    private void OnInventorySlotClick()
    {
        //드래그에 사용할 이미지 위치 슬롯 아이템 이미지 위치로 변경
        dragItemImage.transform.position = fromSlot.transform.position; 
        dragItemImage.gameObject.SetActive(true);
        dragItemImage.sprite = fromSlot.CurSlotItem.ItemImage;
        fromSlot.OnDragScaleEvent?.Invoke();
    }

    /// <summary>
    /// 아이템 클릭 후 마우스 뗐을 때 동작
    /// </summary>
    private void OnInventorySlotMouseUp()
    {
        dragItemImage.gameObject.SetActive(false);
        fromSlot.OnResetScaleEvent?.Invoke();
    }

    /// <summary>
    /// 아이템 상세 UI 설정
    /// </summary>
    private void SetSlotItemDetail()
    {
        //사용 가능한 아이템이면 버튼 활성화
        equipButton.gameObject.SetActive(!fromSlotItem && fromSlot.CurSlotItem.IsEquipment);
        
        detailItemImage.gameObject.SetActive(!fromSlotItem);
        detailItemDescA.gameObject.SetActive(!fromSlotItem);
        detailItemDescB.gameObject.SetActive(!fromSlotItem);
    }
    
}
