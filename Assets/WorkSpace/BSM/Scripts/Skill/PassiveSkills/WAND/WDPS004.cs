using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDPS004 : PassiveSkill
{
    public WDPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        
        CriticalRateBuff(skillNode.skillData.SkillAbilityValue, skillNode.skillData.SkillAbilityFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        CriticalRateBuff(-skillNode.skillData.SkillAbilityValue, -skillNode.skillData.SkillAbilityFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
