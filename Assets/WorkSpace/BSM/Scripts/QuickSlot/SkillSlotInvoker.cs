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
    public float InvokeSkillFromSlot(QuickSlotType quickSlotType)
    {
        SkillNode skillNode = QuickSlotData.GetQuickSlotSkillData(quickSlotType);

        if (skillNode != null)
        {
            slotSkill = SkillFactoryManager.GetSkillFactory(skillNode);
            slotSkill.UseSkill();
            return skillNode.skillData.RecoveryTime;
        } 
        
        return 0;
    } 
}
