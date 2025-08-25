using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS002 : MeleeSkill
{
    public SPAS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        MultiEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, 0, 180f, 90f, 270f);
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            multiActions.Add(new List<Action>());
            
            for (int i = 0; i < 2; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                multiActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                multiActions[0].Add(() => ExecuteKnockBack(playerPosition, direction, receiver));
                
                triggerModules[0].AddCollider(cols[i]);
            }
            multiActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(multiActions[0]);
        }
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
