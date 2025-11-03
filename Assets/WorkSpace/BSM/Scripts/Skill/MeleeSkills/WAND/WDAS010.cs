using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS010 : MeleeSkill
{
    private Action action;
    private Vector2 hitPos;
    private Coroutine castingCo;
    private Coroutine delayCo;

    public WDAS010(SkillNode skillNode) : base(skillNode)
    {
        leftHash = Animator.StringToHash("SkillMotion_Left_2");
        rightHash = Animator.StringToHash("SkillMotion_Right_2");
        upHash = Animator.StringToHash("SkillMotion_Up_2");
        downHash = Animator.StringToHash("SkillMotion_Down_2");
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        skillDamage = GetSkillDamage();
        
        hitPos = playerPosition + (direction * (skillNode.skillData.SkillRadiusRange / 2));
        action = () => ExecutePostCastAction(direction, playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    private void ExecutePostCastAction(Vector2 direction, Vector2 playerPosition)
    { 
        SpawnParticleAtRandomPosition(hitPos, skillNode.skillData.SkillRadiusRange, 0, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle", 8);
        
        delayCo = skillNode.PlayerSkillReceiver.StartCoroutine(DelayHitRoutine(direction, playerPosition));
    }

    private IEnumerator DelayHitRoutine(Vector2 direction, Vector2 playerPosition)
    {
        yield return WaitCache.GetWait(0.55f);
        
        Collider2D[] cols = Physics2D.OverlapBoxAll(hitPos, overlapSize, 0, monsterLayer);

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
        UnityEngine.Gizmos.color = Color.black;
        UnityEngine.Gizmos.DrawWireCube(hitPos, overlapSize);
    }
}
