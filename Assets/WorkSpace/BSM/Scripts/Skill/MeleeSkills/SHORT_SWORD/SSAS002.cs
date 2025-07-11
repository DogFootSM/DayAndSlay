using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS002 : SkillFactory
{
    public SSAS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 사용");
    }

    public override void ApplyLevelUpStats()
    {
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 스탯 조정");
    }

    public override void Gizmos()
    {
        
    }
}
