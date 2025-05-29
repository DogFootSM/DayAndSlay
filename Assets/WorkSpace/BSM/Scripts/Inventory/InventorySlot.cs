using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item CurSlotItem => curSlotItem;
    private Item curSlotItem;
    
    private Image itemImage;

    private bool isDrag;
    
    private void Awake()
    {
        itemImage = transform.GetChild(1).GetComponent<Image>();

        if (curSlotItem == null)
        {
            itemImage.color = Color.black;
        }
        
    }

    public void AddItem(Item item)
    { 
        curSlotItem = item;
        itemImage.sprite = item.itemData.ItemImage;
    }

    public void ItemClicked()
    {
        isDrag = true;
    }
    
}
