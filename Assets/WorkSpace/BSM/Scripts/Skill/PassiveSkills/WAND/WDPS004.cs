using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDPS004 : PassiveSkill
{
    public WDPS004(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        CriticalRateBuff(8f, 3f);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        CriticalRateBuff(0f, 0f);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
