using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS009 : MeleeSkill
{ 
    public SSAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);

        if (cols.Length > 0)
        {
            skillActions.Add(new List<Action>());
            
            //현재 스킬 사용 시 몬스터 감지 수
            int detectedCount = skillNode.skillData.DetectedCount < cols.Length ? skillNode.skillData.DetectedCount : cols.Length;
            
            //스킬 레벨에 따른 디버프 시간 계산
            float debuffDuration = GetDeBuffDurationIncreasePerLevel(skillNode.skillData.SkillAbilityFactor);
            
            for (int i = 0; i < detectedCount; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                skillActions[0].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
                skillActions[0].Add(() => ExecuteStun(receiver, debuffDuration));
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
    }
}