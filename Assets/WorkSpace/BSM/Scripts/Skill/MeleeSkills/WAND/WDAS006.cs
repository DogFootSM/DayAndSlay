using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS006 : MeleeSkill
{
    public WDAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        Fire(playerPosition, direction);
    }

    private void Fire(Vector2 playerPosition, Vector2 direction)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            
            for (int i = 0; i < cols.Length; i++)
            {
                
                IEffectReceiver receiver = cols[0].GetComponent<IEffectReceiver>();

                SkillEffect(cols[0].transform.position, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                triggerModules[0].AddCollider(cols[0]);
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