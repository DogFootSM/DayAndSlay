using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSPS001 : PassiveSkill
{

    public SSPS001(SkillNode skillNode) : base(skillNode)
    {

    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    { 

    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        Debug.Log("스피드 팩터 적용");
        float moveFactor = skillNode.PlayerModel.PlayerStats.moveSpeed * 0.1f;
        skillNode.PlayerModel.UpdateMoveSpeedFactor(moveFactor);
        
        PassiveEffect();
        float attackFactor = skillNode.PlayerModel.PlayerStats.attackSpeed;
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void RevertPassiveEffects()
    {
        Debug.Log("능력 해제");
    }
    
    public override void Gizmos()
    {
        throw new System.NotImplementedException();
    }
}
