using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS004 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SSAS004(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("LeftSSAS004");
        rightHash = Animator.StringToHash("RightSSAS004");
        upHash = Animator.StringToHash("UpSSAS004");
        downHash = Animator.StringToHash("DownSSAS004");
        
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        skillDamage = GetSkillDamage();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);

        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        Collider2D[] detected = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(detected, playerPosition);
        
        //현재 스킬 레벨당 슬로우 비율 계산
        float slowRatio = skillNode.skillData.SkillAbilityValue +
                          ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);           
        
        if (detected.Length > 0)
        {
            //몬스터 감지 가능 수와 감지한 수 비교 후 타격할 몬스터 수 설정
            int detectedCount = skillNode.skillData.DetectedCount <= detected.Length ? skillNode.skillData.DetectedCount : detected.Length;
            
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monster = detected[i].GetComponent<IEffectReceiver>();
                SkillEffect(detected[i].transform.position + Vector3.up, i, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
                skillActions.Add(new List<Action>());
                skillActions[i].Add(() => ExecuteSlow(monster, skillNode.skillData.DeBuffDuration, slowRatio));
                skillActions[i].Add(() => Hit(monster, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[i].Add(() => RemoveTriggerModuleList(i));
                triggerModules[i].AddCollider(detected[i]);
                interactions[i].ReceiveAction(skillActions[i]); 
            } 
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){ }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
