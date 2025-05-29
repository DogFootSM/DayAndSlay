using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private Image itemImage;
    
    public Item CurSlotItem => curSlotItem;
    private Item curSlotItem;
     
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
    public void AddItem(Item item, int count = 1)
    {  
        curSlotItem = item;
        itemImage.sprite = item.itemData.ItemImage;
        itemCount += count;
        itemCountText.text = $"{itemCount}";
        CountTextActive();
    }
 
    /// <summary>
    /// 중복 아이템일 경우 개수 텍스트 활성화
    /// </summary>
    private void CountTextActive()
    { 
        itemCountText.gameObject.SetActive(curSlotItem != null && curSlotItem.itemData.IsOverlaped);
    }

    /// <summary>
    /// 아이템 이미지 크기 원상복구
    /// </summary>
    private void ScaleReset()
    {
        //TODO: 아이템 드래그 시 어떻게 할지?
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
