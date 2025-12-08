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
        
        SoundManager.Instance.PlaySfx(SFXSound.SPAS005);
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteMovementBlock(skillNode.skillData.BuffDuration);

        //스킬 레벵 당 초당 회복량
        float healthRegenPerLevel = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        //피해량 감소 6% 고정
        ExecuteDamageReduction(skillNode.skillData.BuffDuration, skillNode.skillData.SkillAbilityValue);
        
        //초당 체력 회복 6% + 레벨 당 0.01%;
        ExecuteHealthRegenTick(skillNode.skillData.BuffDuration, healthRegenPerLevel);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
