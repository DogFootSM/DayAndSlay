using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOPS001 : PassiveSkill
{
    private float agilityLevelPer = 0.15f;
    
    public BOPS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;

        float agilityFactor = skillNode.PlayerModel.PlayerStats.baseAgility * agilityLevelPer;
        
        
        Debug.Log($"±âº» :{skillNode.PlayerModel.PlayerStats.baseAgility} , {agilityFactor}");

    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
    }
}
