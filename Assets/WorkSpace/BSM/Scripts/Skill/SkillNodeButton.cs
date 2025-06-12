using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Image skillIconImage;
     
    public SkillNode CurSkillNode;
     
    private void Awake()
    {
        increaseButton.onClick.AddListener(InvestSkillPoint);
        InitSkillIcon(); 
    }
 
    /// <summary>
    /// 스킬 포인트 및 선행 스킬 잠금 해제에 따라 버튼 상호작용 T/F 적용
    /// </summary>
    /// <param name="point"></param>
    public void UpdateSkillButtonState(int point)
    { 
        CurSkillNode.TryUnlockByPrerequisites();
        increaseButton.interactable = point > 0 && CurSkillNode.UnLocked; 
    }
    
    /// <summary>
    /// 스킬 아이콘 이미지 셋팅
    /// </summary>
    private void InitSkillIcon()
    {
        skillIconImage.sprite = CurSkillNode.skillData.SkillIcon;
    }
    
    /// <summary>
    /// 스킬 포인트 투자 후 스킬 강화 진행
    /// </summary>
    private void InvestSkillPoint()
    {
        CurSkillNode.ApplyPoint();
    }
    
}
