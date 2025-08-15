using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS007 : MeleeSkill
{
    public SSAS007(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition + Vector2.up, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        ExecuteAttackUpDefenseDown(skillNode.skillData.BuffDuration, 0.5f, 1.5f);

    }

    public override void ApplyPassiveEffects()
    {
    }

    public override void Gizmos()
    {
    }
}