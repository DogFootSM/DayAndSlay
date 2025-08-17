using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS002 : MeleeSkill
{
    public SSAS002(SkillNode skillNode) : base(skillNode)
    {
        leftDeg = 90f; 
        rightDeg = 270f;
        downDeg = 180f;
        upDeg = 0f;
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        SetOverlapSize(direction, skillNode.skillData.SkillRadiusRange);
        MeleeEffect(playerPosition + direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        SetParticleStartRotationFromDeg(0,direction, leftDeg, rightDeg, downDeg, upDeg);
        ExecuteMoveSpeedBuff(skillNode.skillData.BuffDuration, 0.3f);
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);
        skillDamage = GetSkillDamage();
 
        if (detectedMonster.Length > 0)
        {
            IEffectReceiver monsterReceiver = detectedMonster[0].GetComponent<IEffectReceiver>();
        
            skillActions.Add(() => Hit(monsterReceiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions.Add(RemoveTriggerModuleList);
            
            if (triggerModule.enabled)
            {
                triggerModule.AddCollider(detectedMonster[0]);
                interaction.ReceiveAction(skillActions);
            }
        } 
    }

    public override void ApplyPassiveEffects() {}

    public override void Gizmos()
    {
        
    }
}
