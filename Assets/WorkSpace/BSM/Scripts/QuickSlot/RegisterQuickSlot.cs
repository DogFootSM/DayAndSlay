using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegisterQuickSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI quickSlotName;
    [SerializeField] private Image skillImage;

    public QuickSlotType QuickSlotType;

    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private SkillNode skillNode;

    private void Awake()
    {
        quickSlotName.text = $"{QuickSlotType}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PreviewRegisterSlot();
    }

    /// <summary>
    /// 스킬 퀵슬롯 설정
    /// </summary>
    private void PreviewRegisterSlot()
    {
        skillNode = quickSlotManager.PreviewSkillRegister(this); 
        skillImage.sprite = skillNode.skillData.SkillIcon;
    }
    
    /// <summary>
    /// 스킬 퀵슬롯 해제
    /// </summary>
    public void PreviewUnRegisterSlot()
    {
        skillImage.sprite = null;
        skillNode = null;
    }
    
}