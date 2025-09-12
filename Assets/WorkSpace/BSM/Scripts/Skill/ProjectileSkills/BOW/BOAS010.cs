using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS010 : MeleeSkill
{
    public BOAS010(SkillNode skillNode) : base(skillNode)
    {
    }

    private Vector2 pos;
    private Vector2 dir;

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();

        pos = playerPosition;
        dir = direction;

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * skillNode.skillData.SkillRange), overlapSize, 0, monsterLayer);
        
        
        
        
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawWireCube(pos + (dir * skillNode.skillData.SkillRange), overlapSize);
    }
}