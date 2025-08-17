using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS009 : MeleeSkill
{
    public SSAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            IEffectReceiver receiver = cols[0].GetComponent<IEffectReceiver>();
            
            skillActions.Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(() => ExecuteStun(receiver, skillNode.skillData.DeBuffDuration));
            skillActions.Add(RemoveTriggerModuleList);

            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(cols[0]);
                interaction.ReceiveAction(skillActions);  
            } 
        } 
    }

    public override void ApplyPassiveEffects()
    {
    }

    public override void Gizmos()
    {
    }
}