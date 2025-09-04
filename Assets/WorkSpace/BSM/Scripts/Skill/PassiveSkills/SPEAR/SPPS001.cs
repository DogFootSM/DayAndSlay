using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS001 : PassiveSkill
{  
    public SPPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        
        
        
        
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
    }
}
