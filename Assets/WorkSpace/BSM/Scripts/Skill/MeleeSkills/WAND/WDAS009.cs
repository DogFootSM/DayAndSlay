using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS009 : MeleeSkill
{
    public WDAS009(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        
        
        skillNode.PlayerSkillReceiver.StartCoroutine(skillNode.PlayerSkillReceiver.RemoveCastingTimeCoroutine(skillNode.skillData.BuffDuration));
    }


    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
