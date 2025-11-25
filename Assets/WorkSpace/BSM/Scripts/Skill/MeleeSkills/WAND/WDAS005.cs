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
        float buffFactor = skillNode.skillData.SkillAbilityValue +
                           ((skillNode.CurSkillLevel - 1) * skillNode.skillData.SkillAbilityFactor);
        
        ExecuteShield(shieldCount, buffFactor, skillNode.skillData.BuffDuration, skillNode.skillData.SkillEffectPrefab[0], $"{skillNode.skillData.SkillId}_1_Particle");
    }
    
    
    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
