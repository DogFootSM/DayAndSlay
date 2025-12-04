using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS008 : MeleeSkill
{
    public BOAS008(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);

        float buffFactor = skillNode.skillData.SkillAbilityValue +
                           ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        ExecuteMoveSpeedBuff(skillNode.skillData.BuffDuration, buffFactor, BuffType.BOW_MOVESPEED);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
