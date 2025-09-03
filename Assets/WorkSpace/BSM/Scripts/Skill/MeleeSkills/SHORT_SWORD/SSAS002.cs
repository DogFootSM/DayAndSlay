using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS002 : MeleeSkill
{
    public SSAS002(SkillNode skillNode) : base(skillNode)
    {
        leftDeg = 90f; 
        rightDeg = 270f;
        downDeg = 180f;
        upDeg = 0f;
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        ListClear();
        
        SetOverlapSize(direction, skillNode.skillData.SkillRadiusRange);
        SkillEffect(playerPosition + direction, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, leftDeg, rightDeg, downDeg, upDeg);
        ExecuteMoveSpeedBuff(skillNode.skillData.BuffDuration, 0.3f);
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);
        skillDamage = GetSkillDamage();
 
        if (detectedMonster.Length > 0)
        {
            skillActions.Add(new List<Action>());
 
            for (int i = 0; i < 1; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => Hit(monsterReceiver, skillDamage, skillNode.skillData.SkillHitCount));

                if (triggerModules[0].enabled)
                {
                    triggerModules[0].AddCollider(detectedMonster[i]);
                    interactions[0].ReceiveAction(skillActions[0]);
                }
            }
             
            skillActions[0].Add(() => RemoveTriggerModuleList(0)); 
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType) {}

    public override void Gizmos()
    {
        
    }
}
