using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOPS001 : PassiveSkill
{
    private float baseAgility = 0.15f;

    private float baseAttackSpeedFactor = 0.1f;
    private float levelAttackSpeedFactor = 0.03f;
    
    public BOPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        AgilityBuff(baseAgility);
        AttackSpeedBuff(baseAttackSpeedFactor, levelAttackSpeedFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        AgilityBuff(0);
        AttackSpeedBuff(0, 0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
