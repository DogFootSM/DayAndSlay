using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BOAS001 : MeleeSkill
{ 
    public BOAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            int detected = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
            
            for (int i = 0; i < detected; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteKnockBack(playerPosition, direction, receiver));
                triggerModules[0].AddCollider(cols[i]);
            }
        
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]);
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
