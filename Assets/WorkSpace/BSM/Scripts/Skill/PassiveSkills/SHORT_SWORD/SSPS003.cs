using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS003 : PassiveSkill
{
    private float damageReflectBaseFactor = 0.08f;
    private float damageReflectLevelFactor = 0.03f;
    
    public SSPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;

        DamageReflectRateBuff(damageReflectBaseFactor, damageReflectLevelFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers(); 
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        DamageReflectRateBuff(0, 0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
