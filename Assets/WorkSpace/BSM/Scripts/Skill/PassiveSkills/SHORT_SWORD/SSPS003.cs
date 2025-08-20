using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS003 : PassiveSkill
{
    public SSPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        skillNode.PlayerModel.PlayerStats.DamageReflectValue = 8f;
        skillNode.PlayerModel.PlayerStats.DamageReflectRate = skillNode.CurSkillLevel * 3f;
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.PlayerStats.DamageReflectValue = 0;
        skillNode.PlayerModel.PlayerStats.DamageReflectRate = 0;
    }
}
