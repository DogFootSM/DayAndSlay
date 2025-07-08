using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSet : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TextMeshProUGUI skillCurLevelText;
    [SerializeField] private TextMeshProUGUI skillMaxLevelText;
    [SerializeField] private GraphicRaycaster skillRaycaster;
    
    private List<RaycastResult> results = new List<RaycastResult>();
    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    public SkillNode CurSkillNode;
     
    private void Awake()
    {
        increaseButton.onClick.AddListener(InvestSkillPoint);
        InitSkill(); 
    }
    
    /// <summary>
    /// 스킬 포인트 및 선행 스킬 잠금 해제에 따라 버튼 상호작용 T/F 적용
    /// </summary>
    /// <param name="point"></param>
    public void UpdateSkillButtonState(int point)
    { 
        CurSkillNode.TryUnlockByPrerequisites();
        increaseButton.interactable = point > 0 && CurSkillNode.UnLocked && CurSkillNode.CurSkillLevel < CurSkillNode.skillData.SkillMaxLevel; 
    }
    
    /// <summary>
    /// 스킬 아이콘 이미지 셋팅
    /// </summary>
    private void InitSkill()
    {
        skillIconImage.sprite = CurSkillNode.skillData.SkillIcon;
        UpdateSkillLevelUI();
    }
    
    /// <summary>
    /// 스킬 포인트 투자 후 스킬 강화 진행
    /// </summary>
    private void InvestSkillPoint()
    {
        CurSkillNode.ApplyPoint();
        UpdateSkillLevelUI();
    }

    /// <summary>
    /// 현재 스킬 레벨에 따른 UI 정보 업데이트
    /// </summary>
    private void UpdateSkillLevelUI()
    {
        //TODO: 스킬 레벨 관련 UI 설정
        if (CurSkillNode.CurSkillLevel < CurSkillNode.skillData.SkillMaxLevel)
        {
            skillCurLevelText.text = $"{CurSkillNode.CurSkillLevel}";
            skillMaxLevelText.text = $"/ {CurSkillNode.skillData.SkillMaxLevel}";
        }
        else
        {
            skillCurLevelText.text = "Max";
            skillMaxLevelText.gameObject.SetActive(false);
        } 
    } 
    
    public void OnPointerClick(PointerEventData eventData)
    { 
        quickSlotManager.UpdateSkillPreviewTab(CurSkillNode);
    }
}
