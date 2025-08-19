using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class SSAS010 : MeleeSkill
{
    public SSAS010(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        multiActions.Clear();
        mainModules.Clear();
        triggerModules.Clear();
        interactions.Clear();
        
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        MultiEffect(playerPosition + new Vector2(0f, 0.7f), 0,$"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {   
            multiActions.Clear();
            mainModules.Clear();
            triggerModules.Clear();
            interactions.Clear();
            
            //TODO: 몬스터 감지 카운트만큼 반복
            for (int i = 0; i < cols.Length; i++)
            {
                MultiEffect(cols[i].transform.position, i,$"{skillNode.skillData.SkillId}_2_Particle",skillNode.skillData.SkillEffectPrefab[1]);

                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                multiActions.Add(new List<Action>());
                multiActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                multiActions[i].Add(() => RemoveTriggerModuleList(i));

                if (triggerModules[i].enabled)
                {
                    triggerModules[i].AddCollider(cols[i]);
                    interactions[i].ReceiveAction(multiActions[i]);
                }
            }   
        }
        

    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}