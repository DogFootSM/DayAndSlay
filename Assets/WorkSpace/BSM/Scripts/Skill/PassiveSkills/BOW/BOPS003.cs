using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOPS003 : PassiveSkill
{
    private float baseMoveSpeedFactor = 0.15f;
    private float levelFactor = 0.05f;
    
    public BOPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        MoveSpeedBuff(baseMoveSpeedFactor, levelFactor);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        MoveSpeedBuff(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
