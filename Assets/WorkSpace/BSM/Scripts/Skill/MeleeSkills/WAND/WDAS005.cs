using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WDAS005 : MeleeSkill
{
    private int shieldCount = 2; 

    private Action action;
    private Coroutine castingCo;
    
    public WDAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);

        action = ExecutePostCastAction;
        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action)); 
    }

    private void ExecutePostCastAction()
    {
        ExecuteFollowCharacterWithParticle(skillNode.skillData.SkillEffectPrefab[0], skillNode.skillData.BuffDuration,$"{skillNode.skillData.SkillId}_1_Particle");
        ExecuteShield(shieldCount, skillNode.skillData.SkillAbilityValue, skillNode.skillData.BuffDuration);
    }
    
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
