using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOAS006 : MeleeSkill
{
    private float restoreHealthByPercent = 0.1f;
    private float restoreHealthLevelPer = 0.05f;
    private float healthPer;
    public BOAS006(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ListClear();
        SkillEffect(playerPosition + Vector2.up, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        healthPer = restoreHealthByPercent + ((skillNode.CurSkillLevel - 1) * restoreHealthLevelPer);
        
        ExecuteRestoreHealthByPercent(healthPer);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
