using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS006 : MeleeSkill
{
    private float deBuffRatio = 0.3f;
    
    public SSAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();

        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            multiActions.Add(new List<Action>());
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(1);
            
            for (int i = 0; i < 1; i++)
            {
                MultiEffect(cols[i].transform.position - new Vector3(0, 0.5f), i, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[i]);
                
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                multiActions[i].Add(() => ExecuteDefenseDeBuff(receiver, deBuffDuration, deBuffRatio));
                multiActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                triggerModules[i].AddCollider(cols[0]);
            }

            multiActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(multiActions[0]);
        }
        else
        {
            Vector2 offset = new Vector2();
            
            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) offset = new Vector2(skillNode.skillData.SkillRange, 0);
            else offset = new Vector2(0, skillNode.skillData.SkillRange);
            
            MeleeEffect((playerPosition + (direction * offset)), skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        }
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}