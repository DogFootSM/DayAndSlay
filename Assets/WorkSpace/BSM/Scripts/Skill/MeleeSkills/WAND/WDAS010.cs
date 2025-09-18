using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS010 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;
    private Coroutine delayCo;

    public WDAS010(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        action = () => ExecutePostCastAction(direction, playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    private void ExecutePostCastAction(Vector2 direction, Vector2 playerPosition)
    { 
        SpawnParticleAtRandomPosition(playerPosition + (direction * (skillNode.skillData.SkillRange / 2)), skillNode.skillData.SkillRadiusRange, 0, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle", 8);
        
        delayCo = skillNode.PlayerSkillReceiver.StartCoroutine(DelayHitRoutine(direction, playerPosition));
    }

    private IEnumerator DelayHitRoutine(Vector2 direction, Vector2 playerPosition)
    {
        yield return WaitCache.GetWait(0.55f);
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition + (direction * (skillNode.skillData.SkillRange / 2)),
            overlapSize, 0, monsterLayer);

        for (int i = 0; i < cols.Length; i++)
        {
            IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
            Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
        } 
    }
        
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
        //UnityEngine.Gizmos.color = Color.black;
        //UnityEngine.Gizmos.DrawWireCube(pos + (dir * (skillNode.skillData.SkillRange / 2)), overlapSize);
    }
}
