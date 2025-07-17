using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS003 : MeleeSkill
{
    public SSAS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition, direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
    }

    public override void ApplyPassiveEffects()
    {
         
    }

    public override void Gizmos()
    {
         
    }

    public override float GetSkillDamage()
    {
        return 0;
    }
}
