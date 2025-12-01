using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDPS002 : PassiveSkill
{
    public WDPS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        TeleportCooldownBuff(0.1f, 0.1f);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        TeleportCooldownBuff(0, 0);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
