using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS003 : MeleeSkill
{
    public SSAS003(SkillNode skillNode) : base(skillNode)
    {
        leftDeg = 180f; 
        rightDeg = 0f;
        downDeg = 270f;
        upDeg = 90f; 
    }

    private Vector2 dir;
    private Vector2 pos;

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        MeleeEffect(playerPosition, direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        SetParticleStartRotationFromDeg(rightDeg, leftDeg, upDeg, downDeg);
        SetParticleRotationX(direction);
        skillDamage = GetSkillDamage();
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);

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
 
    private void SetParticleRotationX(Vector2 direction)
    {
        if (direction.x < 0) ChangeRotationToDeg(leftDeg, 0f);
        
        if (direction.x > 0) ChangeRotationToDeg(rightDeg, 0f);

        if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y < 0) ChangeRotationToDeg(0, leftDeg); 
            else ChangeRotationToDeg(0, rightDeg); 
        }
    }

    private void ChangeRotationToDeg(float xAngle, float yAngle)
    {
        mainModule.startRotationX = Mathf.Deg2Rad * xAngle;
        mainModule.startRotationY = Mathf.Deg2Rad * yAngle;
    }
    
    public override void ApplyPassiveEffects()
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }
}