using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS005 : MeleeSkill
{

    public SSAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        skillActions.Clear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        
        for (int i = 0; i < skillNode.skillData.SkillEffectPrefab.Count; i++)
        {
            GameObject instance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_{i+1}_Particle", skillNode.skillData.SkillEffectPrefab[i]);
            particlePooling.ReturnSkillParticlePool($"{skillNode.skillData.SkillId}_{i+1}_Particle", instance);
            ParticleSystem particleSystem = instance.GetComponent<ParticleSystem>();
            mainModule = particleSystem.main;
            
            if (i == 0)
            {
                SetParticleStartRotationFromDeg(direction,180f, 0f, 270f, 90f);
            }
            else
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    instance.transform.localScale = new Vector2(1.8f, 0.1f);
                }
                else
                {
                    instance.transform.localScale = new Vector2(0.1f, 1.8f); 
                }
                
                SetParticleStartRotationFromDeg(direction,0, 180f, 90f, 270f);
            }
        }
        
        ExecuteDash(direction.normalized);
        MeleeEffect(playerPosition - (direction / 2), skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            IEffectReceiver monster = cols[0].GetComponent<IEffectReceiver>();
            skillActions.Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(() => DashStunEffect(monster, skillNode.skillData.DeBuffDuration));
            skillActions.Add(RemoveTriggerModuleList);
            
            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(cols[0]);
                interaction.ReceiveAction(skillActions);
            }
        } 
    }

    public override void ApplyPassiveEffects(){}

    public override void Gizmos(){}
}
