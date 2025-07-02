using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class RegisterTableSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    
    [Inject] private TableManager tableManager;
    
    private ItemData itemData;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
    }

    public void SetRegisterItem(ItemData itemData)
    {
        this.itemData = itemData;
        itemImage.sprite = itemData.ItemImage; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //선택한 아이템 전달
    }
}
