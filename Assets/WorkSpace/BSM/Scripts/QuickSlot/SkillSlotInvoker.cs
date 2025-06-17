using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillSlotInvoker : MonoBehaviour
{
    private SkillFactory slotSkill;

    private Vector2 curDirection;
    public UnityAction<Vector2> OnDirectionChanged;

    private void OnEnable()
    {
        OnDirectionChanged += ChangedDirection;
    }

    private void OnDisable()
    {
        OnDirectionChanged -= ChangedDirection;
    }

    /// <summary>
    /// 스킬을 사용 시 타격될 방향 변경
    /// </summary>
    /// <param name="direction">스킬 사용 시 데미지를 줄 몬스터를 감지할 방향</param>
    private void ChangedDirection(Vector2 direction)
    {
        curDirection = direction;
    }
  
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
            slotSkill.UseSkill(curDirection, transform.position);
            return skillNode.skillData.RecoveryTime;
        } 
        
        return 0;
    } 
}
