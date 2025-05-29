using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemCountText;
    
    public Item CurSlotItem => curSlotItem;
    private Item curSlotItem;
    
    private Image itemImage;
     
    private int itemCount = 0;
    private bool isDrag;
    
    private void Awake()
    {
        Init();
        CountTextActive();
    }

    private void Init()
    {
        itemImage = transform.GetChild(1).GetComponent<Image>();

        if (curSlotItem == null)
        {
            itemImage.color = Color.black;
        }
        
        itemCountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
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
    
}
