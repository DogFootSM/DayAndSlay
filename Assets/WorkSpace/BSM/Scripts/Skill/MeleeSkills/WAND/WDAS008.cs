using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS008 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;
    
    public WDAS008(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SetOverlapSize(skillNode.skillData.SkillRadiusRange);
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        skillDamage = GetSkillDamage();

        action = () => ExecutePostCastAction(playerPosition);
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action));
    }

    private void ExecutePostCastAction(Vector2 playerPosition)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(playerPosition, overlapSize, 0, monsterLayer);
        
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);

        if (cols.Length > 0)
        {
            SpawnParticleAtRandomPosition(playerPosition, skillNode.skillData.SkillRadiusRange, 0.5f, skillNode.skillData.SkillEffectPrefab[1], $"{skillNode.skillData.SkillId}_2_Particle", 10);

            for (int i = 0; i < cols.Length; i++)
            {
                IEffectReceiver receiver = cols[i].GetComponent<IEffectReceiver>();
                Hit(receiver, skillDamage, skillNode.skillData.SkillHitCount);
                ExecuteDot(receiver, skillNode.skillData.DeBuffDuration, 1f, skillDamage * 0.2f);
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
