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
        //DB Load 시 현재 착용중인 무기가 패시브 스킬의 무기와 같을 때에만 패시브 능력 적용
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;
        PassiveEffect();
        
        //현재 기본 이동 속도 기준 Factor 만큼 증가한 값
        float moveFactor = skillNode.PlayerModel.PlayerStats.moveSpeed * 0.1f;
        skillNode.PlayerModel.UpdateMoveSpeedFactor(moveFactor);
        
        //현재 스킬 레벨 당 추가 수치
        float perLevel = skillNode.CurSkillLevel * 0.02f;
        
        //현재 기본 공격 속도 기준 Factor + 레벨 당 추가 수치만큼 증가한 값
        float attackFactor = (skillNode.PlayerModel.PlayerStats.attackSpeed * 0.08f) + perLevel;
        skillNode.PlayerModel.UpdateAttackSpeedFactor(attackFactor);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateMoveSpeedFactor(0);
        skillNode.PlayerModel.UpdateAttackSpeedFactor(0);
    }
    
    public override void Gizmos()
    {
    }
}
