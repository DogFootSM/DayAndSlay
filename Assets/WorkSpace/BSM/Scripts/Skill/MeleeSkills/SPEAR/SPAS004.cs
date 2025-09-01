using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS004 : MeleeSkill
{
    public SPAS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);

        MultiEffect(playerPosition + (direction * 4f), 0, $"{skillNode.skillData.SkillId}_1_Particle",
            skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, 90f, 270f, 180f, 0);
        SetParticleLocalScale(new Vector3(3f, 1.5f), new Vector3(1.5f, 3f));
        
        if (direction.y > 0)
        {
            particleSystemRenderer.sortingOrder = -1;
        }
        else
        {
            particleSystemRenderer.sortingOrder = 50;
        }
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction,
            overlapSize, 0, monsterLayer);

        skillDamage = GetSkillDamage();

        if (cols.Length > 0)
        {
            multiActions.Add(new List<Action>());

            for (int i = 0; i < 1; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                multiActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                
                //TODO: DeBuffDuration 시간만큼 1초당 Truncate 값 만큼 지속 딜
                multiActions[0].Add(() => ExecuteDot(receiver, skillNode.skillData.DeBuffDuration, 1f,
                    MathF.Truncate(receiver.GetMaxHp() * (0.05f * skillNode.CurSkillLevel))));
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