using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS003 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SPAS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        Vector2 offset = new Vector2();
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            offset = new Vector2(0f, 0.25f);
        }
        else
        {
            offset = new Vector2(-0.25f, 0f);
        }

        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        SkillEffect(playerPosition + offset + (direction * 1.55f), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        //파티클 회전 및 크기 수정
        SetParticleStartRotationFromDeg(0, direction, 0, 180f, 90f, 270f);
        SetParticleLocalScale(new Vector3(1.5f, 1f), new Vector3(1f, 1.5f));
        
        if (direction.x < 0 || direction.y < 0)
        {
            SetParticleRendererFlip(Vector3.up);
        }
        else
        {
            SetParticleRendererFlip(Vector3.zero);
        }
        
        particleSystemRenderer.sortingOrder = direction.y > 0 ? -1 : 50;
        
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;

            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();

                skillActions[0].Add(() => Hit(receiver, skillDamage + GeteDefensePenetrationDamage(receiver), skillNode.skillData.SkillHitCount));
                
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
        UnityEngine.Gizmos.color =Color.red;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
