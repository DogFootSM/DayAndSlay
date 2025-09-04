using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS004 : PassiveSkill
{
    public SSPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        float resistancePer = 12f + ((skillNode.CurSkillLevel -1) * 6f);
        
        skillNode.PlayerModel.UpdateResistanceFactor(resistancePer);
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateResistanceFactor(0);
    }
}
