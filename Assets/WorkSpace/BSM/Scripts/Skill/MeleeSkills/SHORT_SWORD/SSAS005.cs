using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS005 : MeleeSkill
{

    public SSAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        GameObject instance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        particlePooling.ReturnSkillParticlePool($"{skillNode.skillData.SkillId}_2_Particle", instance);
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            instance.transform.localScale = new Vector2(1.8f, 0.1f);
        }
        else
        {
            instance.transform.localScale = new Vector2(0.1f, 1.8f); 
        }   
        
        multiActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
        
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
         
        MultiEffect(playerPosition - (direction / 2), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,180f, 0f, 270f, 90f);
        
        multiActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
        MultiEffect(playerPosition - (direction / 2), 0, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        SetParticleStartRotationFromDeg(0, direction,0, 180f, 90f, 270f);
        ExecuteDash(direction.normalized);
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(0.2f);
            multiActions.Add(new List<Action>());
             
            for (int i = 0; i < 1; i++)
            {
                IEffectReceiver monster = cols[i].GetComponent<IEffectReceiver>();
                multiActions[0].Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
                multiActions[0].Add(() => ExecuteDashStun(monster, deBuffDuration));
                triggerModules[0].AddCollider(cols[i]);
            }
            
            multiActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(multiActions[0]);
        }
    }

    public override void ApplyPassiveEffects(){}

    public override void Gizmos(){}
}
