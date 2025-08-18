using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS001 : PassiveSkill
{

    public SSPS001(SkillNode skillNode) : base(skillNode)
    {
        float moveFactor = skillNode.PlayerModel.PlayerStats.moveSpeed * 0.1f;

        skillNode.PlayerModel.UpdateMoveSpeedFactor(moveFactor);
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 

    }

    public override void ApplyPassiveEffects()
    {
        PassiveEffect();
        float attackFactor = skillNode.PlayerModel.PlayerStats.attackSpeed;

        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
        throw new System.NotImplementedException();
    }
}
