using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS003 : PassiveSkill
{
    private float defenseFactor = 0.15f;
    private float defenseLevelModifier = 0.02f;
    
    public SPPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        float defenseLevelPer = defenseFactor + ((skillNode.CurSkillLevel - 1) * defenseLevelModifier);
        
        skillNode.PlayerModel.UpdateDefenseFactor(defenseLevelPer);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateDefenseFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
