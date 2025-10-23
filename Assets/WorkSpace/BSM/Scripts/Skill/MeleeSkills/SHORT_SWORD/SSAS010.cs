using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class SSAS010 : MeleeSkill
{
    public SSAS010(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("LeftSSAS010");
        rightHash = Animator.StringToHash("RightSSAS010");
        upHash = Animator.StringToHash("UpSSAS010");
        downHash = Animator.StringToHash("DownSSAS010");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        Test(playerPosition);
    }

    private void Test(Vector2 playerPosition)
    {
        skillNode.PlayerSkillReceiver.StartCoroutine(TestRoutine(playerPosition));
    }

    private IEnumerator TestRoutine(Vector2 playerPosition)
    {
        yield return WaitCache.GetWait(0.5f);
        
        SkillEffect(playerPosition + new Vector2(0f, 0.7f), 0,$"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        skillDamage = GetSkillDamage();
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {   
            ListClear();
            
            //현재 스킬의 최대 감지 가능한 몬스터 수
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
 
            for (int i = 0; i < detectedCount; i++)
            {
                SkillEffect(cols[i].transform.position, i,$"{skillNode.skillData.SkillId}_2_Particle",skillNode.skillData.SkillEffectPrefab[1]);

                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                skillActions.Add(new List<Action>());
                skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[i].Add(() => RemoveTriggerModuleList(i));

                if (triggerModules[i].enabled)
                {
                    triggerModules[i].AddCollider(cols[i]);
                    interactions[i].ReceiveAction(skillActions[i]);
                }
            }   
        } 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}