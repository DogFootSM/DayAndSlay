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
        ListClear();
        SoundManager.Instance.PlaySfx(SFXSound.SPAS008_02);
        SkillEffect(playerPosition + new Vector2(0, 1.5f), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        float moveSpeedDecrease = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        float defenseIncrease = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        ExecuteDefenseUpSpeedDown(skillNode.skillData.BuffDuration, moveSpeedDecrease, defenseIncrease);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}