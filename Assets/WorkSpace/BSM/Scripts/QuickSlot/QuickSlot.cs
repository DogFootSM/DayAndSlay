using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotTypeText;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private QuickSlotType quickSlotType;
    
    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private SkillNode curSlotSkillNode;
    public SkillNode CurrentSlotSkillNode => curSlotSkillNode;
    public QuickSlotType CurrentQuickSlotType => quickSlotType;
    
    private void Awake()
    {
        Init();  
    }

    private void Init()
    {
        slotTypeText.text = $"{quickSlotType}";  
        QuickSlotData.AddQuickSlotEntry(quickSlotType, this);
    }

    /// <summary>
    /// 메인 퀵슬롯 설정
    /// </summary>
    /// <param name="skillNode"></param>
    public void ApplySkillToMainQuickSlot(SkillNode skillNode)
    {
        curSlotSkillNode = skillNode;
        skillIconImage.sprite = curSlotSkillNode.skillData.SkillIcon;
    }

    /// <summary>
    /// 메인 퀵슬롯 설정 해제
    /// </summary>
    public void UnassignExistingQuickSlotForSkill()
    { 
        curSlotSkillNode = null;
        skillIconImage.sprite = null;
    }
    
}
