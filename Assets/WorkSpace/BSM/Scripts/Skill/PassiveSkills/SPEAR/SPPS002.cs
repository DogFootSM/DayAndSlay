using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS002 : PassiveSkill
{
    private float moveSpeedFactor = 0.08f;
    private float attackSpeedFactor = 0.08f;
    private float attackSpeedLevelPer = 0.02f;
    
    public SPPS002(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
    }

    public override void ApplyPassiveEffects(CharacterWeaponType weaponType)
    {
        if (weaponType != skillNode.PlayerModel.ModelCurWeaponType) return;

        //현재 기본 이동 속도 기준 Factor 만큼 증가한 값
        MoveSpeedBuff(moveSpeedFactor);
        
        //현재 기본 공격 속도에 Factor + 스킬 레벨당 추가 수치만큼 증가한 값
        AttackSpeedBuff(attackSpeedFactor, attackSpeedLevelPer);
        
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }

    public override void Gizmos()
    {
    }

    public override void RevertPassiveEffects()
    {
        skillNode.PlayerModel.UpdateMoveSpeedFactor(0);
        skillNode.PlayerModel.UpdateAttackSpeedFactor(0);
        skillNode.PlayerModel.ApplyPassiveSkillModifiers();
    }
}
