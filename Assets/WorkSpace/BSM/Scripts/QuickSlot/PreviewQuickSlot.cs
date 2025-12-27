using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PreviewQuickSlot : MonoBehaviour, 
    IPointerClickHandler
{
    [SerializeField] private QuickSlotType curSlotType;
    [SerializeField] private TextMeshProUGUI slotTypeText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private Sprite baseSkillIconSprite;

    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    
    
    private void Awake()
    {
        slotTypeText.text = $"{curSlotType}"; 
    }

    public void SetPreviewSlot(SkillNode skillNode = null)
    { 
        if (skillNode == null)
        {
            skillIconImage.sprite = baseSkillIconSprite;
        }
        else
        {
            skillIconImage.sprite = skillNode.skillData.SkillIcon;
        } 
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySfx(SFXSound.SLOTDROP);
        quickSlotManager.RegisteredSkillNode(curSlotType);
    }
}
