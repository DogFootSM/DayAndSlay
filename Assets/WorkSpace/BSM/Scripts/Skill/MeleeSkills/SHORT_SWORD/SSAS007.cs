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
        ListClear();
        SkillEffect(playerPosition + new Vector2(0, 1.5f), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        float defenseDecrease = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        float attackIncrease = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        ExecuteAttackUpDefenseDown(skillNode.skillData.BuffDuration, defenseDecrease, attackIncrease);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}