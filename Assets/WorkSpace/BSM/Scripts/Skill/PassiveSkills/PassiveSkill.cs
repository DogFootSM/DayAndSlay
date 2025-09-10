using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : SkillFactory
{ 
    public PassiveSkill(SkillNode skillNode) : base(skillNode)
    {
    }

    protected void PassiveEffect()
    {
        //TODO: 패시브 이펙트 적용 안할거면 메서드 제거하기
    }

    /// <summary>
    /// 기본 이동 속도 * factor만큼 MoveSpeed에 적용
    /// </summary>
    /// <param name="factor"></param>
    protected void MoveSpeedBuff(float factor)
    {
        float moveFactor = skillNode.PlayerModel.PlayerStats.baseMoveSpeed * factor;
        skillNode.PlayerModel.UpdateMoveSpeedFactor(moveFactor);
    }
    
    /// <summary>
    /// 기본 이동 속도 * (기본 Factor + 레벨당 추가 Factor) 공격 속도에 적용
    /// </summary>
    /// <param name="baseFactor">스킬 기본 증가값</param>
    /// <param name="levelFactor">레벨당 추가로 증가할 값</param>
    protected void AttackSpeedBuff(float baseFactor, float levelFactor)
    {
        //현재 스킬 레벨당 추가 Factor
        float skillLevelPerStats = (skillNode.CurSkillLevel - 1) * levelFactor;
        
        //기본 Factor + 레벨당 추가 Factor 값에 기본 공격 속도를 곱한 증가할 공격 속도 값
        float attackFactor = (skillNode.PlayerModel.PlayerStats.baseAttackSpeed * (baseFactor + skillLevelPerStats));
        skillNode.PlayerModel.UpdateAttackSpeedFactor(attackFactor);
    }

    /// <summary>
    /// 공격력 증가 버프
    /// 기본 공격력 * factor 값만큼 공격력 증가
    /// </summary>
    /// <param name="baseFactor">기본 스킬 증가값</param>
    protected void StrengthBuff(float baseFactor)
    {
        float strengthFactor = skillNode.PlayerModel.PlayerStats.baseStrength * baseFactor;
        skillNode.PlayerModel.UpdateStrengthFactor(strengthFactor);
    }

    /// <summary>
    /// 크리티컬 확률 증가 버프
    /// 기본 크리티컬 확률 + factor 만큼 증가한 값
    /// </summary>
    /// <param name="baseFactor">스킬 기본 증가 factor</param>
    /// <param name="levelFactor">레벨당 추가 증가 factor</param>
    protected void CriticalBuff(float baseFactor, float levelFactor)
    {
        float criticalFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateCriticalPerFactor(criticalFactor);
    }
    
    public abstract void RevertPassiveEffects();

}
