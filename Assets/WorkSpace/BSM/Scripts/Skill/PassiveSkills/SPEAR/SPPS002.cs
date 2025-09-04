using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPS002 : PassiveSkill
{
    private float moveSpeedValueModifier = 0.08f;
    private float attackSpeedValueModifier = 0.08f;
    private float attackSpeedLevelModifier = 0.02f;
    
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
        float moveFactor = skillNode.PlayerModel.PlayerStats.baseMoveSpeed * moveSpeedValueModifier;
        skillNode.PlayerModel.UpdateMoveSpeedFactor(moveFactor);
        
        //현재 스킬 레벨 당 추가 수치
        float perLevel = (skillNode.CurSkillLevel - 1) * attackSpeedLevelModifier;
        
        //현재 기본 공격 속도 기준 Factor + 레벨 당 추가 수치만큼 증가한 값
        float attackFactor = (skillNode.PlayerModel.PlayerStats.baseAttackSpeed * attackSpeedValueModifier) + perLevel;

        skillNode.PlayerModel.UpdateAttackSpeedFactor(attackFactor);
        
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
