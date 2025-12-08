using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SSAS001 : MeleeSkill
{
    private Vector2 hitPos; 
   
    public SSAS001(SkillNode skillNode) : base(skillNode)
    { 
        leftDeg = 270f; 
        rightDeg = 90f;
        downDeg = 0f;
        upDeg = 180f;
        
        leftHash = Animator.StringToHash("SkillMotion_Left_1");
        rightHash = Animator.StringToHash("SkillMotion_Right_1");
        upHash = Animator.StringToHash("SkillMotion_Up_1");
        downHash = Animator.StringToHash("SkillMotion_Down_1");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {   
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        SoundManager.Instance.PlaySfx(SFXSound.SSAS001);
        SkillEffect(hitPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction, leftDeg, rightDeg, downDeg, upDeg);

        if (direction.x > 0 || direction.y > 0)
        {
            particleSystemRenderer.flip = new Vector3(1f, 0, 0);
        }
        else
        {
            particleSystemRenderer.flip = new Vector3(0, 0, 0);
        }
          
        Collider2D[] detectedMonster = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0f, monsterLayer);
        Sort.SortMonstersByNearest(detectedMonster, playerPosition);
        
        if (detectedMonster.Length > 0)
        {
            skillActions.Add(new List<Action>());
            int detectedCount = 
                skillNode.skillData.DetectedCount <= detectedMonster.Length
                ? skillNode.skillData.DetectedCount
                : detectedMonster.Length;
            
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monsterReceiver = detectedMonster[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => ExecuteKnockBack(playerPosition, monsterReceiver));
                skillActions[0].Add(() => Hit(monsterReceiver ,skillDamage, skillNode.skillData.SkillHitCount));
                
                triggerModules[0].AddCollider(detectedMonster[i]);
                interactions[0].ReceiveAction(skillActions[0]); 
            }
             
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
        } 
    }
 
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType) { }
    
    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}