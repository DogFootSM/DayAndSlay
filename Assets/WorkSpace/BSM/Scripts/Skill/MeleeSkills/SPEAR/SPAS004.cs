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
        GameObject instance = particlePooling.GetSkillPool($"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        particlePooling.ReturnSkillParticlePool($"{skillNode.skillData.SkillId}_1_Particle", instance);

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            instance.transform.localScale = new Vector3(1.5f, 1f);
        }
        else
        {
            instance.transform.localScale = new Vector3(1f, 1.5f);
        }

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * skillNode.skillData.SkillRange),
            overlapSize, 0, monsterLayer);

        skillDamage = GetSkillDamage();

        if (cols.Length > 0)
        {
            Vector2 pos = cols[cols.Length / 2].transform.position;
            
            //TODO: 창 소환 위치 어떻게 할까 애매하네;
            MultiEffect(playerPosition + (direction * 2f), 0, $"{skillNode.skillData.SkillId}_1_Particle",
                skillNode.skillData.SkillEffectPrefab[0]);
            
            multiActions.Add(new List<Action>());
            
            for (int i = 0; i < 1f; i++)
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
        else
        {
            MultiEffect(playerPosition + (direction * 2f), 0, $"{skillNode.skillData.SkillId}_1_Particle",
                skillNode.skillData.SkillEffectPrefab[0]);
        }
        
        if (direction.y > 0)
        {
            particleSystemRenderer.sortingOrder = -1;
        }
        else
        {
            particleSystemRenderer.sortingOrder = 50;
        }
        
        SetParticleStartRotationFromDeg(0, direction, 90f, 270f, 180f, 0);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}