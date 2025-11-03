using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SSAS005 : MeleeSkill
{
    private Vector2 hitPos;
    
    public SSAS005(SkillNode skillNode) : base(skillNode)
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
        
        SkillEffect(hitPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        SetParticleStartRotationFromDeg(0, direction,180f, 0f, 270f, 90f); 
        SetParticleLocalScale(new Vector3(1.8f, 0.1f), new Vector3(0.1f, 1.8f));
        
        ExecuteDash(direction.normalized);
        ListClear(); 
        SkillEffect(playerPosition - (direction / 2), 0, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
        SetParticleStartRotationFromDeg(0, direction,0, 180f, 90f, 270f);
        SetParticleLocalScale(new Vector3(2.8f, 0.1f), new Vector3(0.1f, 2.8f));
         
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);
        
        Test(cols);
    }

    private void Test(Collider2D[] cols)
    {
        skillNode.PlayerSkillReceiver.StartCoroutine(TestRoutine(cols));
    }

    private IEnumerator TestRoutine(Collider2D[] cols)
    {
        yield return WaitCache.GetWait(0.25f);
       
        if (cols.Length > 0)
        {
            //현재 스킬 레벨당 디버스 지속 시간 증가
            float deBuffDuration = GetDeBuffDurationIncreasePerLevel(skillNode.skillData.SkillAbilityFactor);
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;

            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver monster = cols[i].GetComponent<IEffectReceiver>();
                Hit(monster, skillDamage, skillNode.skillData.SkillHitCount);
                ExecuteDashStun(monster, deBuffDuration);
            }
        }
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType){}

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
