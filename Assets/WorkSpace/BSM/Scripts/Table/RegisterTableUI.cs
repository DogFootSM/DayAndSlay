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
    [SerializeField] private Button panelCloseButton;
    
    [Inject] private TableManager tableManager;
    
    public UnityAction<ItemData> OnRegisterItemEvents;
    public UnityAction OnRegisterUIResetEvents;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        
        registerItemButton.onClick.AddListener(tableManager.Register);
        panelCloseButton.onClick.AddListener(() => panelCloseButton.gameObject.transform.parent.gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        OnRegisterItemEvents += OnRegisterItem;
        OnRegisterUIResetEvents += ResetRegisterTableUI;
    }

    private void OnDisable()
    {
        OnRegisterItemEvents -= OnRegisterItem; 
        OnRegisterUIResetEvents -= ResetRegisterTableUI;
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

    private void ResetRegisterTableUI()
    {
        registerItemImage.sprite = null;
        registerItemText.text = "";
    }
}
