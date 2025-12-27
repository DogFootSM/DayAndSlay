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
    
    private Sprite defaultSprite;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        
        registerItemButton.onClick.AddListener(tableManager.Register);
        panelCloseButton.onClick.AddListener(() => panelCloseButton.gameObject.transform.parent.gameObject.SetActive(false));
        
        defaultSprite = registerItemImage.sprite;
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

    /// <summary>
    /// History : 2025.12.19
    /// 작성자 : 이재호
    /// itemImage의 스프라이트를 기본 스프라이트로 변경
    /// </summary>
    private void ResetRegisterTableUI()
    {
        registerItemImage.sprite = defaultSprite;
        registerItemText.text = "";
    }
}
