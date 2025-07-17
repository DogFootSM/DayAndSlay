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
        MeleeEffect(playerPosition, direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 사용");
    }

    public override void ApplyPassiveEffects() {}

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
