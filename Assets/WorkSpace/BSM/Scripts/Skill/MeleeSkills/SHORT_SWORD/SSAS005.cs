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
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
         
        MultiEffect(playerPosition - (direction / 2), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,180f, 0f, 270f, 90f); 
        SetParticleLocalScale(new Vector3(1.8f, 0.1f), new Vector3(0.1f, 1.8f));
        
        ListClear();
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

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){}

    public override void Gizmos(){}
}
