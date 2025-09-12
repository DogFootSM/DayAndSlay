using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDAS005 : MeleeSkill
{
    private int shieldCount = 2;
    private int skillDuration = 30;
    public WDAS005(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        ExecuteCasting(skillNode.skillData.SkillCastingTime);
        
        ListClear();
        SkillEffect(playerPosition, 0, $"{skillNode.skillData.SkillId}_1_Particle", skillNode.skillData.SkillEffectPrefab[0]);
        ExecuteShield(shieldCount, skillNode.skillData.SkillAbilityValue, skillDuration);
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
    }

    public override void Gizmos()
    {
    }
}
