using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS008 : MeleeSkill
{
    public SSAS008(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition + Vector2.up, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        ExecuteDefenseUpSpeedDown(skillNode.skillData.BuffDuration, 0.5f, 2f);
    }

    public override void ApplyPassiveEffects()
    {
    }

    public override void Gizmos()
    {
    }
}