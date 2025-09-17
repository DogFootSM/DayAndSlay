using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS006 : MeleeSkill
{
    private Action action;
    private Vector2 targetPos;
    
    private Coroutine castingCo;
    private Collider2D targetCollider;
    
    private float minDistance = 99999f;
    
    public WDAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        SetOverlapSize(direction, skillNode.skillData.SkillRange);
        skillDamage = GetSkillDamage();
        
        action = () => ExecutePostCastAction(playerPosition, direction);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action));

    }

    private void ExecutePostCastAction(Vector2 playerPosition, Vector2 direction)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * (skillNode.skillData.SkillRange / 2)),
            overlapSize, 0, monsterLayer);

        //감지된 적이 없으면 return
        if (cols.Length < 1) return;
        
        //가장 가까운 적 위치 탐색
        for (int i = 0; i < cols.Length; i++)
        {
            if (Vector2.Distance(playerPosition, cols[i].transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(playerPosition, cols[i].transform.position);
                targetPos = cols[i].transform.position;
                targetCollider = cols[i];
            } 
        }

        IEffectReceiver receiver = targetCollider.GetComponent<IEffectReceiver>();
        
        SkillEffect(targetPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        //UnityEngine.Gizmos.color = Color.red;
        //UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * (skillNode.skillData.SkillRange /2)), overlapSize);
    }
}