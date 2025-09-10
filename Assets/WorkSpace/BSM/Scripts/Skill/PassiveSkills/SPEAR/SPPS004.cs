using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS004 : PassiveSkill
{
    private float baseArmorPenetration = 0.1f;
    private float levelArmorPenetration = 0.05f;
    
    public SPPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        ArmorPenetrationBuff(baseArmorPenetration, levelArmorPenetration);    
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        ArmorPenetrationBuff(0, 0);
    }
}
