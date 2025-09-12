using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDPS001 : PassiveSkill
{
    private float inteligenceFactor = 15f;
    public WDPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        
        IntelligenceBuff(inteligenceFactor);
        SkillAttackBuff(skillNode.skillData.SkillAbilityFactor, skillNode.skillData.SkillAbilityValue);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        IntelligenceBuff(-inteligenceFactor);
        SkillAttackBuff(0, 0);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
