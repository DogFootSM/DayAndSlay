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
    private SkillData skillData;

    public SkillData SkillData
    {
        get => skillData;
        set => skillData = value;
    }

    private void Awake()
    {
        quickSlotName.text = $"{QuickSlotType}";
        quickSlotManager.AddRegisterQuickSlotEntry(QuickSlotType, this);
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
        skillData = quickSlotManager.PreviewSkillRegister(this);

        skillImage.sprite = skillData.SkillIcon;
    }

    /// <summary>
    /// 스킬 퀵슬롯 아이콘 이미지 해제
    /// </summary>
    public void PreviewUnRegisterSlot()
    {
        skillImage.sprite = null;
    }

    public void UpdateRegisterSlot(Sprite newSprite)
    {
        skillImage.sprite = newSprite;
    }
}