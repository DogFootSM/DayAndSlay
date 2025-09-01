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
        MultiEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillName}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        //현재 스킬 레벨 당 +10% 추가
        float multiplier = 0.5f + ((skillNode.CurSkillLevel - 1) * 0.1f);
        ExecuteNextSkillDamageBuff(skillNode.skillData.BuffDuration, multiplier);
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
