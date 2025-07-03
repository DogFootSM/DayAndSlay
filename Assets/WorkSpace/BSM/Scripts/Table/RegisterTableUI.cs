using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class RegisterTableUI : MonoBehaviour
{
    [SerializeField] private Image registerItemImage;
    [SerializeField] private TextMeshProUGUI registerItemText;
    [SerializeField] private Button registerItemButton;

    [Inject] private TableManager tableManager;
    
    public UnityAction<ItemData> OnRegisterItemEvents;

    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        
        registerItemButton.onClick.AddListener(tableManager.Register);
    }

    private void OnEnable()
    {
        OnRegisterItemEvents += OnRegisterItem;
    }

    private void OnDisable()
    {
        OnRegisterItemEvents -= OnRegisterItem; 
    }
    
    /// <summary>
    /// 전달 받은 아이템 데이터로 정보 갱신
    /// </summary>
    /// <param name="item">슬롯에서 선택한 아이템 데이터</param>
    private void OnRegisterItem(ItemData item)
    { 
        registerItemImage.sprite = item.ItemImage;
        registerItemText.text = item.Name;
    } 
}
