using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS006 : MeleeSkill
{
    private float healthPer;
    public BOAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SoundManager.Instance.PlaySfx(SFXSound.BOAS006);
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        healthPer = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        ExecuteRestoreHealthByPercent(healthPer);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
