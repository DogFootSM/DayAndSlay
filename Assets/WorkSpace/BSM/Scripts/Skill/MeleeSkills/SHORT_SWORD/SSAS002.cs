using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS002 : MeleeSkill
{
    private Vector2 hitPos;
    
    private int leftHash = Animator.StringToHash("LeftSSAS002");
    private int rightHash = Animator.StringToHash("RightSSAS002");
    private int upHash = Animator.StringToHash("UpSSAS002");
    private int downHash = Animator.StringToHash("DownSSAS002");
    
    
    public SSAS002(SkillNode skillNode) : base(skillNode)
    {
        leftDeg = 90f; 
        rightDeg = 270f;
        downDeg = 180f;
        upDeg = 0f;
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        
        //타격 위치 및 이펙트 재생 위치
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRadiusRange / 2));
        
        //이펙트 재생
        SkillEffect(hitPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        //바라보는 방향에 따른 이펙트 회전
        SetParticleStartRotationFromDeg(0, direction, leftDeg, rightDeg, downDeg, upDeg);

        if (direction.x < 0 || direction.y < 0)
        {
            particleSystemRenderer.flip = new Vector3(1f, 0, 0);
        }
        else
        {
            particleSystemRenderer.flip = new Vector3(0, 0, 0);
        }
        
        
        //이동 속도 증가 버프
        float speedBuffFactor = skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        ExecuteMoveSpeedBuff(skillNode.skillData.BuffDuration, speedBuffFactor);
        
        Collider2D[] detectedMonster = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0f, monsterLayer);
        Sort.SortMonstersByNearest(detectedMonster, playerPosition);
        
        if (detectedMonster.Length > 0)
        {
            int detectedCount = skillNode.skillData.DetectedCount <= detectedMonster.Length ? skillNode.skillData.DetectedCount : detectedMonster.Length;
            
            skillActions.Add(new List<Action>());
 
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => Hit(monsterReceiver, skillDamage, skillNode.skillData.SkillHitCount));
                triggerModules[0].AddCollider(detectedMonster[i]);
            }
            
            skillActions[0].Add(() => RemoveTriggerModuleList(0)); 
            interactions[0].ReceiveAction(skillActions[0]);
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType) {}

    public override int SendSkillAnimationHash(Vector2 direction)
    {
        if (direction == Vector2.right) return rightHash;
        if (direction == -Vector2.right) return leftHash;
        if (direction == Vector2.up) return upHash;
        if (direction == -Vector2.up) return downHash;

        return 0;
    }
    
    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
