using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS005 : MeleeSkill
{
    public SPAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        MultiEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteMovementBlock(skillNode.skillData.BuffDuration);

        float skillValuePerLevel = skillNode.CurSkillLevel * 0.06f;
        ExecuteDamageReduction(skillNode.skillData.BuffDuration, skillValuePerLevel);
        ExecuteHealthRegenTick(skillNode.skillData.BuffDuration, skillValuePerLevel);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
