using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainQuickSlot : MonoBehaviour
{
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI quickSlotTypeText;
    [SerializeField] private QuickSlotType curQuickSlotType;

    private void Awake()
    {
        quickSlotTypeText.text = $"{curQuickSlotType}";
    }

    public void SetMainQuickSlot(SkillNode skillNode = null)
    {
        if (skillNode == null)
        {
            skillIconImage.sprite = null;
        }
        else
        {
            skillIconImage.sprite = skillNode.skillData.SkillIcon;
        }
    }
    
}
