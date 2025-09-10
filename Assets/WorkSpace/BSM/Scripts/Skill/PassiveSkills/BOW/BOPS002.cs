using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOPS002 : PassiveSkill
{
    private float baseCriticalRate = 0.08f;
    private float levelCriticalRate = 0.03f;
    
    public BOPS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        CriticalRateBuff(baseCriticalRate, levelCriticalRate);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        CriticalRateBuff(0, 0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
