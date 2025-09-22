using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS003 : MeleeSkill
{
    private Vector2 hitPos;
 
    public SSAS003(SkillNode skillNode) : base(skillNode)
    {
        leftDeg = 180f; 
        rightDeg = 0f;
        downDeg = 270f;
        upDeg = 90f; 
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        SkillEffect(hitPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,rightDeg, leftDeg, upDeg, downDeg);
        SetParticleRotationX(direction);
        skillDamage = GetSkillDamage();
        
        Collider2D[] detectedMonster = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0f, monsterLayer);
        
        if (detectedMonster.Length > 0)
        {
            skillActions.Add(new List<Action>());
            
            int detectedCount = skillNode.skillData.DetectedCount <= detectedMonster.Length ? skillNode.skillData.DetectedCount : detectedMonster.Length;
            
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();
                
                skillActions[0].Add(() => Hit(monsterReceiver, skillDamage + GetDefensePenetrationDamage(monsterReceiver), skillNode.skillData.SkillHitCount));
                triggerModules[0].AddCollider(detectedMonster[i]);
            }
            
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]);
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
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}