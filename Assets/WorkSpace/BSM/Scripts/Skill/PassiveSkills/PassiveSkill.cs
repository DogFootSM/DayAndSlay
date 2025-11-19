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
    }

    /// <summary>
    /// 기본 이동 속도 * factor만큼 MoveSpeed에 적용
    /// </summary>
    /// <param name="factor">기본 이동 속도 증가 값</param>
    /// <param name="levelFactor">레벨당 추가 증가값</param>
    protected void MoveSpeedBuff(float factor, float levelFactor = 0)
    {
        float moveFactor = factor + ((skillNode.CurSkillLevel - 1) * levelFactor);
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
        float attackFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        
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
    /// 지능 증가 버프
    /// 기본 지능 + factor 값만큼 지능 증가
    /// </summary>
    /// <param name="baseFactor"></param>
    protected void IntelligenceBuff(float baseFactor)
    {
        float intelligenceFactor = skillNode.PlayerModel.PlayerStats.baseIntelligence * baseFactor;
        skillNode.PlayerModel.UpdateIntelligenceFactor(intelligenceFactor);
    }

    /// <summary>
    /// 마법 공격력 증가 버프
    /// 기본 마공 * factor만큼 마공 증가
    /// </summary>
    /// <param name="baseFactor"></param>
    protected void SkillAttackBuff(float baseFactor, float levelFactor)
    {   
        float skillLevelPerStats = (skillNode.CurSkillLevel - 1) * levelFactor;
        
        //기본 Factor + 레벨당 추가 Factor 값에 기본 공격 속도를 곱한 증가할 공격 속도 값
        float skillAttackFactor = (skillNode.PlayerModel.PlayerStats.SkillAttack * (baseFactor + skillLevelPerStats));
        //모델에 해당 함수 추가해야함
        //skillNode.PlayerModel.UpdateSkillAttackFactor(skillAttackFactor);
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

    protected void ReduceCastingTimeBuff(float baseFactor, float levelFactor)
    {
        float reductionFactor = baseFactor + ((skillNode.CurSkillLevel - 1) * levelFactor);
        skillNode.PlayerModel.UpdateCastingTimeReduction(reductionFactor);
    }

    protected void TeleportCooldownBuff(float baseFactor, float levelFactor)
    {
        float skillLevelPerStats = baseFactor + (skillNode.CurSkillLevel - 1) * levelFactor;
        //플레이어 모델에 추가해야함
        //skillNode.PlayerModel.UpdateDodgeCooldown(skillLevelPerStats);
    }
    
    public abstract void RevertPassiveEffects();

}
