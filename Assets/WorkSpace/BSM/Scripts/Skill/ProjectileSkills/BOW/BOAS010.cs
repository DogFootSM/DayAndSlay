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
        
        
        
        
        // if (cols.Length > 0)
        // { 
        //     skillActions.Add(new List<Action>()); 
        //     for (int i = 0; i < cols.Length; i++)
        //     {
        //         IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
        //         
        //         skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
        //         triggerModules[0].AddCollider(cols[i]); 
        //     }
        //     
        //     skillActions[0].Add(() => RemoveTriggerModuleList(0));
        //     interactions[0].ReceiveAction(skillActions[0]);
        // }
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