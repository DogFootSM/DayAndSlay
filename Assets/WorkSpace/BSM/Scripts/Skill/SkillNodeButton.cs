using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNodeButton : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Image skillIconImage;
    
    public SkillNode CurSkillNode;

    private void Awake()
    {
        increaseButton.onClick.AddListener(InvestSkillPoint);
        InitSkillIcon();
    }
    
    
    /// <summary>
    /// 스킬 아이콘 이미지 셋팅
    /// </summary>
    private void InitSkillIcon()
    {
        skillIconImage.sprite = CurSkillNode.skillData.SkillIcon;
    }
    
    /// <summary>
    /// 스킬 포인트 투자 후 스킬 강화
    /// </summary>
    private void InvestSkillPoint()
    {
        Debug.Log($"현재 스킬 :{CurSkillNode.skillData.SkillId}");
        CurSkillNode.Test();
        
    }
    
}
