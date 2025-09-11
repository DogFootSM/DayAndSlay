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
    /// <param name="factor">기본 이동 속도 증가 값</param>
    /// <param name="levelFactor">레벨당 추가 증가값</param>
    protected void MoveSpeedBuff(float factor, float levelFactor = 0)
    {
        float perLevel = (skillNode.CurSkillLevel - 1) * levelFactor;
        float moveFactor = skillNode.PlayerModel.PlayerStats.baseMoveSpeed * (factor + perLevel);
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
    protected void CriticalRateBuff(float baseFactor, float levelFactor)
    {
        float criticalFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateCriticalPerFactor(criticalFactor);
    }

    /// <summary>
    /// 크리티컬 데미지 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 증가 데미지</param>
    /// <param name="levelFactor">레벨당 추가 데미지</param>
    protected void CriticalDamageBuff(float baseFactor, float levelFactor)
    {
        float criticalDamageFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateCriticalDamage(criticalDamageFactor);        
    }
    
    /// <summary>
    /// 피해 반사 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 피해 반사</param>
    /// <param name="levelFactor">레벨당 추가 피해 반사</param>
    protected void DamageReflectRateBuff(float baseFactor, float levelFactor)
    {
        skillNode.PlayerModel.PlayerStats.DamageReflectRate =
            baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
    }

    /// <summary>
    /// 상태 이상 내성 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 상태 이상 내성</param>
    /// <param name="levelFactor">레벨당 추가 상태 이상 내성</param>
    protected void ResistanceBuff(float baseFactor, float levelFactor)
    {
        float resistanceFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateResistanceFactor(resistanceFactor);
    }

    /// <summary>
    /// 방어력 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 방어력 증가율</param>
    /// <param name="levelFactor">레벨당 추가 증가율</param>
    protected void DefenseBuff(float baseFactor, float levelFactor)
    {
        float defenseFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateDefenseFactor(defenseFactor);
    }

    /// <summary>
    /// 방어력 관통 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 방어력 관통력</param>
    /// <param name="levelFactor">레벵당 추가 방어력 관통력</param>
    protected void ArmorPenetrationBuff(float baseFactor, float levelFactor)
    {
        float armorPenetration = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateArmorPenetration(armorPenetration);
    }

    /// <summary>
    /// 민첩 증가 버프
    /// </summary>
    /// <param name="baseFactor">기본 민첩 증가량</param>
    protected void AgilityBuff(float baseFactor)
    {
        float agilityFactor = skillNode.PlayerModel.PlayerStats.baseAgility * baseFactor;
        skillNode.PlayerModel.UpdateAgilityStats(agilityFactor);
    }
    
    public abstract void RevertPassiveEffects();

}
