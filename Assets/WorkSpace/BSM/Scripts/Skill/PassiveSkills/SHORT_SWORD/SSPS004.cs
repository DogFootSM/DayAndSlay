using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS004 : PassiveSkill
{
    private float baseResistance = 0.12f;
    private float resistanceLevel = 0.06f;
    
    public SSPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        float resistancePer = baseResistance + ((skillNode.CurSkillLevel -1) * resistanceLevel);
        
        skillNode.PlayerModel.UpdateResistanceFactor(resistancePer);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateResistanceFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
