using System;
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
        ListClear();

        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        SkillEffect(playerPosition + direction, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,rightDeg, leftDeg, upDeg, downDeg);
        SetParticleRotationX(direction);
        skillDamage = GetSkillDamage();
        
        Collider2D[] detectedMonster =
            Physics2D.OverlapBoxAll(playerPosition + direction, overlapSize, 0f, monsterLayer);

        if (detectedMonster.Length > 0)
        {
            skillActions.Add(new List<Action>());
            
            for (int i = 0; i < 1; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();

                skillActions[0].Add(() => Hit(monsterReceiver, skillDamage, skillNode.skillData.SkillHitCount));
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

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }
}