using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SSAS001 : MeleeSkill
{
    private Vector2 dir;
    private Vector2 pos; 
    
    public SSAS001(SkillNode skillNode) : base(skillNode)
    { 
        leftDeg = 270f; 
        rightDeg = 90f;
        downDeg = 0f;
        upDeg = 180f;
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {   
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        MeleeEffect(playerPosition + direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        SetParticleStartRotationFromDeg(direction, leftDeg, rightDeg, downDeg, upDeg);
        skillDamage = GetSkillDamage();
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);
   
        if (detectedMonster.Length < 1) return;
 
        IEffectReceiver monsterReceiver = detectedMonster[0].GetComponent<IEffectReceiver>();
        
        skillActions.Add(() => ExecuteKnockBack(playerPosition, direction, monsterReceiver));
        skillActions.Add(() => Hit(monsterReceiver ,skillDamage, skillNode.skillData.SkillHitCount));
        skillActions.Add(RemoveTriggerModuleList);
        
        if (triggerModule.enabled)
        {
            triggerModule.AddCollider(detectedMonster[0]);
            interaction.ReceiveAction(skillActions);
        } 
    }
    
    public override void ApplyPassiveEffects() { }
    
    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }
}