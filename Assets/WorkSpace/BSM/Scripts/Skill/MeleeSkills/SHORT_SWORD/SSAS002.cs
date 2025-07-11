using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS002 : MeleeSkill
{
    public SSAS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 사용");
    }

    public override float GetSkillDamage()
    {
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 스탯 조정");

        float damage = skillNode.CurSkillLevel * skillNode.skillData.SkillDamage;
         
        return damage;
    }

    public override void Gizmos()
    {
        
    }
}
