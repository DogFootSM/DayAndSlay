using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS009 : MeleeSkill
{
    private Action action;
    private Coroutine castingCo;
    
    public WDAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        action = () => ExecutePostCastAction(playerPosition);

        castingCo = skillNode.PlayerSkillReceiver.StartCoroutine(WaitCastingRoutine(action));
    }

    private void ExecutePostCastAction(Vector2 playerPosition)
    {
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteRemoveCast(skillNode.skillData.BuffDuration);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
