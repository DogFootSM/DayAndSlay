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
using Zenject;

public class InventoryInteraction :
    InventoryController,
    IPointerClickHandler,
    ISavable
{
    [SerializeField] GraphicRaycaster inventoryRayCanvas;
    [SerializeField] private Image dragItemImage;
    [SerializeField] private Image detailItemImage;
    [SerializeField] private TextMeshProUGUI detailItemDescA;
    [SerializeField] private TextMeshProUGUI detailItemDescB;
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI equipStateButtonText;
    [SerializeField] private SystemWindowController systemWindowController;
    [SerializeField] private ScrollRect inventoryScrollRect;
    [SerializeField] private CustomScrollRect customScrollRect;

    [Inject] private SaveManager saveManager;
    
    private HashSet<int> ownedItemSet = new HashSet<int>();
    private List<RaycastResult> results = new List<RaycastResult>();
     
    private InventorySlot beginSlot;
    private InventorySlot endSlot;
    private InventorySlot selectedSlot;

    
    private bool beginSlotNullCheck => beginSlot != null && beginSlot.CurSlotItem == null;
    private bool closeInventory => systemWindowController.GetSystemType() != SystemType.INVENTORY;

    new void Awake()
    {
        base.Awake();
        saveManager.SavableRegister(this);
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

    private void OnEnable()
    {
        InventorySlotEvent.OnSlotBeginDrag += HandleBeginDrag;
        InventorySlotEvent.OnSlotDrag += HandleDrag;
        InventorySlotEvent.OnSlotEndDrag += HandleEndDrag;
    }

    private void OnDisable()
    {
        InventorySlotEvent.OnSlotBeginDrag -= HandleBeginDrag;
        InventorySlotEvent.OnSlotDrag -= HandleDrag;
        InventorySlotEvent.OnSlotEndDrag -= HandleEndDrag;    }

    /// <summary>
    /// 인벤토리 슬롯의 장비 아이템 장착, 장착 해제
    /// </summary>
    private void Equip()
    {
        if (selectedSlot.IsEquip)
        {
            equipment.UnEquipItem(selectedSlot.CurSlotItem.Parts);
            selectedSlot.IsEquip = false;
            UpdateEquipButton();
        }
        else
        { 
            equipment.EquipItem(selectedSlot.CurSlotItem);
            UpdateEquipState(selectedSlot); 
            UpdateEquipButton();
        }
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
                if (inventorySlots[i].CurSlotItem == null) continue;

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
    /// 장비 장착, 해제에 따른 버튼 텍스트 갱신
    /// </summary>
    private void UpdateEquipButton()
    {
        equipButton.gameObject.SetActive(selectedSlot.CurSlotItem.IsEquipment);
        
        if (selectedSlot.IsEquip)
        {
            equipStateButtonText.text = "장착 해제";
        }
        else
        {
            equipStateButtonText.text = "장착";
        }  
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (closeInventory) return;
        results.Clear();
        
        inventoryRayCanvas.Raycast(eventData, results);

        if (results.Count < 1) return;
        
        selectedSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();
 
        if (selectedSlot != null && selectedSlot.CurSlotItem != null)
        { 
            detailItemImage.gameObject.SetActive(true);
            detailItemDescA.gameObject.SetActive(true);
            detailItemDescB.gameObject.SetActive(true);
            
            UpdateEquipDetailTab();
            UpdateEquipButton();
        } 
    }

    /// <summary>
    /// 아이템 드래그 시작 동작을 Inventory Slot에게 전달 받음
    /// </summary>
    /// <param name="eventData"></param>
    private void HandleBeginDrag(PointerEventData eventData)
    {
        if (closeInventory) return;
        results.Clear(); 
        CustomScrollRect.allowDrag = true;

        inventoryRayCanvas.Raycast(eventData, results);
        
        if(results.Count < 1) return;
        
        beginSlot = results[0].gameObject.GetComponentInParent<InventorySlot>();

        if (beginSlot != null && beginSlot.CurSlotItem != null)
        { 
            dragItemImage.sprite = beginSlot.CurSlotItem.ItemImage;
            
            if (!dragItemImage.gameObject.activeSelf)
            {
                dragItemImage.gameObject.SetActive(true); 
            }
        } 
    }

    /// <summary>
    /// 아이템 드래그 중인 동작을 Inventory Slot에게 전달 받음
    /// </summary>
    /// <param name="eventData"></param>
    private void HandleDrag(PointerEventData eventData)
    {
        if (beginSlotNullCheck) return;
        if (closeInventory) return;
        
        //마우스가 이동하는 위치로 아이템 이미지 위치 변경
        dragItemImage.transform.position = eventData.position;
    }

    /// <summary>
    /// 아이템 드래그 끝난 동작을 전달 받음
    /// </summary>
    /// <param name="eventData"></param>
    private void HandleEndDrag(PointerEventData eventData)
    {
        if (closeInventory) return;
        
        if (dragItemImage.gameObject.activeSelf)
        {
            dragItemImage.gameObject.SetActive(false);
        }
        CustomScrollRect.allowDrag = false;

        inventoryRayCanvas.Raycast(eventData, results);

        foreach (RaycastResult pde in results)
        {
            endSlot = pde.gameObject.GetComponentInParent<InventorySlot>();

            if (endSlot != null && !endSlot.Equals(beginSlot))
            {
                SwapInventorySlot(endSlot, beginSlot);
                break;
            } 
        }
    }
    
    /// <summary>
    /// 인벤토리 슬롯 스왑
    /// </summary>
    /// <param name="endSlot">마지막으로 감지한 슬롯</param>
    /// <param name="beginSlot">처음 감지한 슬롯</param>
    private void SwapInventorySlot(InventorySlot endSlot, InventorySlot beginSlot)
    {
        if (endSlot == null || beginSlot == null) return;
        
        if (beginSlot.CurSlotItem == null) return;
        
        if (endSlot.CurSlotItem == null)
        {
            Sprite endSlotImage = endSlot.GetComponent<Image>().sprite;
            
            endSlot.ChangeItem(beginSlot.CurSlotItem, beginSlot.ItemCount, beginSlot.CurSlotItem.ItemImage); 
            beginSlot.ChangeItem(null, 0, endSlotImage);
            
            endSlot.IsEquip = beginSlot.IsEquip;
            beginSlot.IsEquip = false;
            UpdateEquipSlot(endSlot); 
            UpdateEquipSlot(beginSlot); 
        }
        else
        { 
            ItemData beginSlotItem = beginSlot.CurSlotItem;
            bool beginEquipState = beginSlot.IsEquip;
            int beginSlotCount = beginSlot.ItemCount;
        
            beginSlot.ChangeItem(endSlot.CurSlotItem, endSlot.ItemCount, endSlot.CurSlotItem.ItemImage);
            beginSlot.IsEquip = endSlot.IsEquip;
             
            endSlot.ChangeItem(beginSlotItem, beginSlotCount, beginSlotItem.ItemImage);
            endSlot.IsEquip = beginEquipState;
        
            UpdateEquipSlot(beginSlot);
            UpdateEquipSlot(endSlot); 
        }  
    }

    /// <summary>
    /// 슬롯 스왑시 장비 착용 상태 및 장비 여부를 확인 후 슬롯 갱신
    /// </summary>
    /// <param name="inventorySlot"></param>
    private void UpdateEquipSlot(InventorySlot inventorySlot)
    {
        if (inventorySlot.CurSlotItem == null) return;
        
        if (inventorySlot.IsEquip && inventorySlot.CurSlotItem.IsEquipment)
        {
            equipSlotDict[inventorySlot.CurSlotItem.Parts] = inventorySlot;
        } 
    }
    
    /// <summary>
    /// 선택한 아이템의 상세 정보 갱신
    /// </summary>
    private void UpdateEquipDetailTab()
    {
        detailItemImage.sprite = selectedSlot.CurSlotItem.ItemImage;
        detailItemDescA.text = selectedSlot.CurSlotItem.ItemDescA;
        detailItemDescB.text = selectedSlot.CurSlotItem.ItemDescB;
    }
    
    /// <summary>
    /// 각 슬롯의 파츠별 아이템 장착 상태 갱신
    /// </summary>
    /// <param name="inventorySlot"></param>
    private void UpdateEquipState(InventorySlot inventorySlot)
    {
        Parts key = inventorySlot.CurSlotItem.Parts;
        
        if (equipSlotDict.ContainsKey(key))
        { 
            equipSlotDict[key].IsEquip = false;
        }
        
        equipSlotDict[key] = inventorySlot;
        equipSlotDict[key].IsEquip = true; 
    }
    
    /// <summary>
    /// 테이블에 아이템 등록 시 사용할 현재 플레이어가 보유중인 아이템 리스트
    /// 현재 슬롯이 장착중이지 않은 장비 아이템과 해당 슬롯을 반환
    /// </summary>
    /// <returns></returns>
    public (List<ItemData>, List<InventorySlot>) GetSlotItemList()
    {
        return (inventorySlots.Where
            (
                x => x.CurSlotItem != null
                     && x.CurSlotItem.IsEquipment
                     && !x.IsEquip).Select(x => x.CurSlotItem).ToList(),
                inventorySlots.Where(
                x => x.CurSlotItem != null
                     && x.CurSlotItem.IsEquipment
                     && !x.IsEquip).ToList()
            );
    }

    /// <summary>
    /// 아이템 데이터 저장
    /// </summary>
    public void Save(SqlManager sqlManager)
    {
        //(ItemID, SlotID, ItemAmount, InventorySlotId, IsEquip)
        List<(string, string, string, string, int)> items = new();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].CurSlotItem != null)
            {
                int equipState = inventorySlots[i].IsEquip ? 1 : 0;
                items.Add(($"{inventorySlots[i].CurSlotItem.ItemId}", $"1", $"{inventorySlots[i].ItemCount}", $"{i}",
                    equipState));
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            sqlManager.UpsertItemDataColumn(
                new[]
                {
                    sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.ITEM_ID),
                    sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.SLOT_ID),
                    sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.ITEM_AMOUNT),
                    sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.INVENTORY_SLOT_ID),
                    sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.IS_EQUIPMENT),
                },
                new[]
                {
                    items[i].Item1, //지급할 Item_id
                    items[i].Item2, //해당 캐릭터 slotId
                    items[i].Item3, //지급할 개수
                    items[i].Item4, //인벤토리 슬롯의 위치
                    $"{items[i].Item5}" //장비 착용 여부
                }
            );
        }
    }

}