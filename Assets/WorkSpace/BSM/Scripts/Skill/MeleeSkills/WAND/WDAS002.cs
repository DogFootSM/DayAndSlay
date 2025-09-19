using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WDAS002 : MeleeSkill
{
    private Coroutine castingCo;
    private Action action;
    public WDAS002(SkillNode skillNode) : base(skillNode)
    {
    }

    private Vector2 pos;
    private Vector2 dir;
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        skillDamage = GetSkillDamage();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        pos = playerPosition;
        dir = direction;
        
        Vector2 particleSpawnPos = SpacingSkillRange(direction, playerPosition);
        Collider2D[] cols = Physics2D.OverlapBoxAll(particleSpawnPos, overlapSize, 0, monsterLayer);
        
        if (cols.Length > 0)
        {
            action = () => ExecutePostCastAction(particleSpawnPos, cols);
            castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action));
        } 
    }

    private void ExecutePostCastAction(Vector2 particleSpawnPos, Collider2D[] cols)
    {
        SkillEffect(particleSpawnPos, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);

        //레벨당 +1 초 지속 시간
        float duration = skillNode.skillData.DeBuffDuration + (skillNode.CurSkillLevel - 1);
        
        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
            ExecuteDot(receiver, duration, 1f, skillDamage / 5);
        }
    }
    
    private Vector2 SpacingSkillRange(Vector2 direction, Vector2 playerPosition)
    {
        return playerPosition + (direction * skillNode.skillData.SkillRange);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawWireCube(pos + (dir * skillNode.skillData.SkillRange), overlapSize);
        
    }
}
