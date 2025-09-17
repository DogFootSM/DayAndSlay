using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SPAS009 : MeleeSkill
{
    private int i = 0;
    
    public SPAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteFollowCharacterWithParticle(skillNode.skillData.SkillEffectPrefab[0], skillNode.skillData.BuffDuration, $"{skillNode.skillData.SkillId}_1_Particle");
        ExecuteFindNearByMonsters(1f, this, skillNode.skillData.BuffDuration);
    }
    
    /// <summary>
    /// 스킬 범위 안에 들어왔을 때 취할 행동
    /// </summary>
    /// <param name="cols">감지한 몬스터 콜라이더 배열</param>
    public void Action(Collider2D[] cols)
    {
        ListClear();
        skillDamage = GetSkillDamage();
        
        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            SkillEffect(cols[i].transform.position + Vector3.up, i, $"{skillNode.skillData.SkillId}_2_Particle", skillNode.skillData.SkillEffectPrefab[1]);
            skillActions.Add(new List<Action>());
            
            skillActions[i].Add(() => Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount));
            skillActions[i].Add(() => RemoveTriggerModuleList(i));
            triggerModules[i].AddCollider(cols[i]);
            interactions[i].ReceiveAction(skillActions[i]);
        } 
    }
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
