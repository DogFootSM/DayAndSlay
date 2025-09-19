using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS005 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SSAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        SkillEffect(hitPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,180f, 0f, 270f, 90f); 
        SetParticleLocalScale(new Vector3(1.8f, 0.1f), new Vector3(0.1f, 1.8f));
 
        ListClear();
        SkillEffect(playerPosition - (direction / 2), 0, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        SetParticleStartRotationFromDeg(0, direction,0, 180f, 90f, 270f);
        SetParticleLocalScale(new Vector3(2.8f, 0.1f), new Vector3(0.1f, 2.8f));
        
        ExecuteDash(direction.normalized);
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            //현재 스킬 레벨당 디버스 지속 시간 증가
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(skillNode.skillData.SkillAbilityFactor);
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
            
            skillActions.Add(new List<Action>());
             
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monster = cols[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteDashStun(monster, deBuffDuration));
                triggerModules[0].AddCollider(cols[i]);
            }
            
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]);
        }
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){}

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
