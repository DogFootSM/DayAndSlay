using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class InventorySlot : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject equippedMark;
    [SerializeField] private Sprite slotOriginImage;

    public ItemData CurSlotItem => curSlotItem;
    private ItemData curSlotItem;
    
    private bool isEquip;
    public bool IsEquip
    {
        get => isEquip;
        set
        {
            isEquip = value;
            equippedMark.SetActive(isEquip);
        }
    }
    
    public int ItemCount => itemCount;
    private int itemCount = 0;
    private Vector3 originScale = new Vector3(1f, 1f, 1f);
    public Action OnResetScaleEvent;
    public Action OnDragScaleEvent;
    
    private void Awake()
    { 
        CountTextActive();
    }

    private void OnEnable()
    {
        OnResetScaleEvent += ScaleReset;
        OnDragScaleEvent += DragScale;
    }

    private void OnDisable()
    {
        OnResetScaleEvent -= ScaleReset;
        OnDragScaleEvent -= DragScale;
    }

    /// <summary>
    /// 슬롯 아이템 추가
    /// </summary>
    /// <param name="item">습득한 아이템</param>
    /// <param name="count">습득한 개수</param>
    public void AddItem(ItemData item, int count = 1)
    {
        curSlotItem = item;
        itemImage.sprite = item.ItemImage;
        itemCount += count;
        CountTextActive();
    }

    /// <summary>
    /// 아이템 교환
    /// </summary>
    /// <param name="item">슬롯에 변경할 아이템</param>
    /// <param name="count">변경할 아이템 개수</param> 
    public void ChangeItem(ItemData item, int count, Sprite sprite)
    {
        curSlotItem = item;
        itemImage.sprite = sprite;
        itemCount = count;
        CountTextActive();
         
        //현재 슬롯에 재료 아이템이 존재하며, 기존에 등록되어 있던 슬롯과 같지 않을 경우 해당 슬롯 할당
        if (curSlotItem != null)
        {
            if (!curSlotItem.IsEquipment)
            { 
                if (InventoryInteraction.OwnedMaterialDict.TryGetValue(curSlotItem.ItemId, out InventorySlot slot))
                {
                    if (!slot.Equals(this))
                    {
                        InventoryInteraction.OwnedMaterialDict[curSlotItem.ItemId] = this;
                    } 
                }
            }
        } 
    }

    /// <summary>
    /// 슬롯 내 아이템 전체 삭제
    /// </summary>
    public void AllRemoveItem()
    {
        if (InventoryInteraction.OwnedMaterialDict.ContainsKey(curSlotItem.ItemId))
        {
            InventoryInteraction.OwnedMaterialDict.Remove(curSlotItem.ItemId);
        }
         
        curSlotItem = null;
        itemImage.sprite = slotOriginImage;
        itemCount = 0;
        CountTextActive();
    }

    /// <summary>
    /// 슬롯 내 아이템 개수만큼 차감
    /// </summary>
    /// <param name="count"></param>
    public void RemoveItem(int count)
    {
        if (itemCount - count < 0) return;
        
        itemCount -= count;

        if (itemCount == 0)
        {
            AllRemoveItem();
            return;
        }
        
        CountTextActive();
    }
    
    /// <summary>
    /// 중복 아이템일 경우 개수 텍스트 활성화
    /// </summary>
    private void CountTextActive()
    {
        itemCountText.text = $"{itemCount}";
        itemCountText.gameObject.SetActive(curSlotItem != null && curSlotItem.IsOverlaped);
    }
    
    /// <summary>
    /// 아이템 이미지 크기 원상복구
    /// </summary>
    private void ScaleReset()
    {
        itemImage.rectTransform.localScale = originScale;
    }

    /// <summary>
    /// 아이템 드래그 시 크기 조정
    /// </summary>
    private void DragScale()
    {
        itemImage.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    
}