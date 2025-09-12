using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WDPS003 : PassiveSkill
{
    public WDPS003(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        ReduceCastingTimeBuff(0.1f, 0.1f);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        ReduceCastingTimeBuff(0, 0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
