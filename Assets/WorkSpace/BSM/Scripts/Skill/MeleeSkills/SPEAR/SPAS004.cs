using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAS004 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SPAS004(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_5");
        rightHash = Animator.StringToHash("SkillMotion_Right_5");
        upHash = Animator.StringToHash("SkillMotion_Up_5");
        downHash = Animator.StringToHash("SkillMotion_Down_5");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRange / 2));
        
        //TODO: 이펙트 재생 위치 상의 필요
        SoundManager.Instance.PlaySfx(SFXSound.SPAS004);
        SkillEffect(playerPosition + (direction * 4f), 0, $"{skillNode.skillData.SkillId}_1_Particle",
            skillNode.skillData.SkillEffectPrefab[0]);
        
        //파티클 회전 및 크기 변경
        SetParticleStartRotationFromDeg(0, direction, 90f, 270f, 180f, 0);
        SetParticleLocalScale(new Vector3(3f, 1.5f), new Vector3(1.5f, 3f));
        
        if (direction.y > 0)
        {
            particleSystemRenderer.sortingOrder = -1;
        }
        else
        {
            particleSystemRenderer.sortingOrder = 50;
        }
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        Sort.SortMonstersByNearest(cols, playerPosition);
        
        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;

            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteDot(receiver, skillNode.skillData.DeBuffDuration, 1f,
                    MathF.Truncate(receiver.GetMaxHp() * (skillNode.skillData.SkillAbilityValue + ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor)))));
                triggerModules[0].AddCollider(cols[i]);
            }
            
            skillActions[0].Add(() => RemoveTriggerModuleList(0));
            interactions[0].ReceiveAction(skillActions[0]); 
        } 
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color =Color.red;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}