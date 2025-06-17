using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotInvoker : MonoBehaviour
{
    private SkillFactory slotSkill;
      
    /// <summary>
    /// 스킬 팩토리에 스킬 노드 전달
    /// </summary>
    /// <param name="quickSlotType">사용할 스킬이 등록된 퀵 슬롯 타입</param>
    /// <returns>해당 스킬의 후딜레이 시간</returns>
    public float SkillInvoke(QuickSlotType quickSlotType)
    {
        SkillNode skillNode = quickSlotType switch
        {
            QuickSlotType.Q => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.Q),
            QuickSlotType.W => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.W),
            QuickSlotType.E => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.E),
            QuickSlotType.R => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.R),
            QuickSlotType.A => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.A),
            QuickSlotType.S => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.S),
            QuickSlotType.D => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.D),
            QuickSlotType.F => QuickSlotData.GetQuickSlotSkillData(QuickSlotType.F),
            _ => null
        };

        if (skillNode != null)
        {
            slotSkill = SkillFactoryManager.GetSkillFactory(skillNode);
            slotSkill.UseSkill();
            return skillNode.skillData.RecoveryTime;
        } 
        
        return 0;
    }
    
}
