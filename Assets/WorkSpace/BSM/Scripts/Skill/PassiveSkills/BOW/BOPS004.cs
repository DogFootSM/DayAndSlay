using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOPS004 : PassiveSkill
{
    private float baseCriticalDamage = 0.07f;
    private float levelFactor = 0.03f;
    
    public BOPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        CriticalDamageBuff(baseCriticalDamage, levelFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        CriticalDamageBuff(0, 0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
