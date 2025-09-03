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
            skillActions.Add(new List<Action>());
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(1);
            
            for (int i = 0; i < 1; i++)
            {
                SkillEffect(cols[i].transform.position - new Vector3(0, 0.5f), i, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[i]);
                
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                skillActions[i].Add(() => ExecuteDefenseDeBuff(receiver, deBuffDuration, deBuffRatio));
                skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                triggerModules[i].AddCollider(cols[0]);
            }

            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]);
        }
        else
        {
            Vector2 offsetWidth = new Vector2();
            Vector2 offsetHeight = new Vector2();
            
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                offsetWidth = new Vector2(skillNode.skillData.SkillRange, 0);
                offsetHeight = playerPosition + (Vector2.down / 2);

            }
            else
            {
                offsetWidth = new Vector2(0, skillNode.skillData.SkillRange);
                offsetHeight = playerPosition;
            }
            
            SkillEffect((offsetHeight + (direction * offsetWidth)), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        }
        
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}