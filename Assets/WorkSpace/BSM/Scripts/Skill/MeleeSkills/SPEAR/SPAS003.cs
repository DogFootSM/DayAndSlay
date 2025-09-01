using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS003 : MeleeSkill
{
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
         
        MultiEffect(playerPosition + offset + (direction * 1.55f), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
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
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            multiActions.Add(new List<Action>());

            for (int i = 0; i < 1; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                multiActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                
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
