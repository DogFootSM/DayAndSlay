using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS007 : MeleeSkill
{
    public WDAS007(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        GetSkillDamage();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        SkillEffect(SpacingSkillRange(direction, playerPosition), 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(SpacingSkillRange(direction, playerPosition), overlapSize, 0, monsterLayer);
                 
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            
            for (int i = 0; i < cols.Length; i++)
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


    public Vector2 SpacingSkillRange(Vector2 direction, Vector2 playerPosition)
    {
        return playerPosition + (direction * skillNode.skillData.SkillRange);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
