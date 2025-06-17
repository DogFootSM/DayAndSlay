using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlotRegisterSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI quickSlotName;
    [SerializeField] private Image skillImage;

    public QuickSlotType QuickSlotType;

    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private SkillData skillData;

    public SkillData SkillData
    {
        get => skillData;
        set => skillData = value;
    }

    private void Awake()
    {
        quickSlotName.text = $"{QuickSlotType}";
        QuickSlotData.AddRegisterQuickSlotEntry(QuickSlotType, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PreviewRegisterSlot();
    }

    /// <summary>
    /// 스킬 퀵슬롯 아이콘 이미지 설정
    /// </summary>
    private void PreviewRegisterSlot()
    {
        skillData = quickSlotManager.RegisterSkillToQuickSlot(this);

        skillImage.sprite = skillData.SkillIcon;
    }

    /// <summary>
    /// 스킬 퀵슬롯 아이콘 이미지 해제
    /// </summary>
    public void PreviewUnRegisterSlot()
    {
        skillImage.sprite = null;
        skillData = null;
    }

    /// <summary>
    /// 스킬 등록할 슬롯 UI 업데이트
    /// </summary>
    /// <param name="skillData"></param>
    public void UpdateRegisterSlotUI(SkillData skillData)
    {
        this.skillData = skillData;
        skillImage.sprite = this.skillData.SkillIcon;
    }
}