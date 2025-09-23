using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS006 : MeleeSkill
{
    public SPAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        //현재 스킬 레벨 당 +10% 추가
        float multiplier = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        ExecuteNextSkillDamageBuff(skillNode.skillData.BuffDuration, multiplier);
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
