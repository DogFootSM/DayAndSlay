using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SPAS001 : MeleeSkill
{
    public SPAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle" , skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, 90f, 270f, 180f, 0);
        if (direction.x > 0 || direction.y > 0)
        {
            SetParticleRendererFlip(Vector3.right);
        }
        else
        {
            SetParticleRendererFlip(Vector3.zero);
        }
        ExecuteDash(direction);
        
        ListClear();
        SkillEffect(playerPosition + direction, 0, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        SetParticleLocalScale(new Vector2(1.5f, 1f), new Vector2(1f, 1.5f));
        SetParticleStartRotationFromDeg(0, direction, 0f, 180f, 90f, 270f);
        if (direction.x < 0 || direction.y < 0) SetParticleRendererFlip(Vector3.up);
        else SetParticleRendererFlip(Vector3.zero);
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            for (int i = 0; i < 2; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
                
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
    }
}
