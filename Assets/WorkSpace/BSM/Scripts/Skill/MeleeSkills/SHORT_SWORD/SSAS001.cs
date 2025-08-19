using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SSAS001 : MeleeSkill
{
    private Vector2 dir;
    private Vector2 pos; 
    
    public SSAS001(SkillNode skillNode) : base(skillNode)
    { 
        leftDeg = 270f; 
        rightDeg = 90f;
        downDeg = 0f;
        upDeg = 180f;
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {   
        multiActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
        
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        MultiEffect(playerPosition + direction, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, leftDeg, rightDeg, downDeg, upDeg);
        skillDamage = GetSkillDamage();
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);

        if (detectedMonster.Length > 0)
        {
            multiActions.Add(new List<Action>());
 
            for (int i = 0; i < 2; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();
                multiActions[0].Add(() => ExecuteKnockBack(playerPosition, direction, monsterReceiver));
                multiActions[0].Add(() => Hit(monsterReceiver ,skillDamage, skillNode.skillData.SkillHitCount));
                
                triggerModules[0].AddCollider(detectedMonster[i]);
                interactions[0].ReceiveAction(multiActions[0]); 
            }
             
            multiActions[0].Add(() => RemoveTriggerModuleList(0));
        } 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType) { }
    
    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }
}