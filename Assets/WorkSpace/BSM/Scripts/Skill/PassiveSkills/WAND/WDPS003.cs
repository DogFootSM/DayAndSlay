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
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        
        //0.1f로 되어있는 스텟을 추후 회피 쿨다운 적용시 그 값변수로 바꿔줘야함
        ReduceCastingTimeBuff(0.1f, 0.1f);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        
        //0.1f로 되어있는 스텟을 추후 회피 쿨다운 적용시 그 값변수로 바꿔줘야함
        ReduceCastingTimeBuff(-0.1f, -0.1f);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
