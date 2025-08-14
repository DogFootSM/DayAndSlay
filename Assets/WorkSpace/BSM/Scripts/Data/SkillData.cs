using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    public string SkillId;
    public string SkillName;
    public List<string> PrerequisiteSkillsId = new();
    public string SkillDescription;
    public List<GameObject> SkillEffectPrefab = new();
    
    public float SkillCooldown;
    public int SkillMaxLevel;
    public float SkillDamage;
    public float SkillDamageIncreaseRate;
    public Sprite SkillIcon;
    public WeaponType RequiredWeapon;
    public float UseSkillDelay;
    public bool IsActive;
    public float UseSkillDelayDecreaseRate;
    public float SkillRange;                                    //스킬 직선 사정거리
    public float SkillRadiusRange;                              
    public int SkillHitCount;
    public float SkillCastingTime;
    public float BuffDuration;
    public float DeBuffDuration;
    
    
    /// <summary>
    /// 스킬 이펙트 오브젝트 설정
    /// </summary>
    private void SetSkillEffect(WeaponType weaponType, string skillId, string effectName)
    {
        //TODO: 추후 스킬 경로 변경해주면 됨
        string[] effects = effectName.Split(',');
        
        for (int i = 0; i < effects.Length; i++)
        {
            Debug.Log(effects[i]);
            
            SkillEffectPrefab.Add(Resources.Load<GameObject>($"SkillEffect/{weaponType}/{skillId}/{effects[i]}"));
        }
        
        //SkillEffectPrefab = Resources.Load<GameObject>($"SkillEffect/{weaponType}/{skillId}/{effectName}");
    }

    private void SetSkillIcon(string iconName)
    {
        SkillIcon = Resources.Load<Sprite>($"SkillIcon/{iconName}");
    }

    private void SetPrerequisiteSkillsId(string prerequisiteSkillsId)
    {
        string[] tmp = prerequisiteSkillsId.Split(',');

        for (int i = 0; i < tmp.Length; i++)
        {
            if(tmp[i].Equals("NONE")) continue;
            
            PrerequisiteSkillsId.Add(tmp[i]);
        } 
    }
    
    /// <summary>
    /// 구글시트 파싱 데이터 설정
    /// </summary>
    /// <param name="skillId">스킬 ID</param>
    /// <param name="skillName">스킬의 이름</param>
    /// <param name="skillDescription">스킬 설명</param>
    /// <param name="skillEffect">스킬 사용 시 설정할 이펙트 이름</param>
    /// <param name="skillCoolDown">스킬 쿨타임</param>
    /// <param name="skillMaxLevel">스킬 최대 레벨</param>
    /// <param name="skillDamage">스킬의 기본 데미지</param>
    /// <param name="skillDamageIncreaseRate">스킬 레벨 당 기본 데미지 계산할 증가율 값</param>
    /// <param name="skillIcon">스킬 아이콘 이미지 설정할 이름</param>
    /// <param name="requiredWeaponType">스킬 장착 필요 무기</param>
    /// <param name="skillDelay">스킬 사용 후 행동 불능 상태의 시간</param>
    /// <param name="skillDelayDecreaseRate">스킬 레벨 당 행동 불능 상태 감소 비율</param>
    /// <param name="skillRange">스킬 공격 시 감지할 몬스터 수</param>
    /// <param name="skillHitCount">스킬 사용 시 몬스터를 타격할 횟수</param>
    public void SetData(string skillId, string skillName, string skillDescription, string skillEffect, 
        float skillCoolDown, int skillMaxLevel, float skillDamage, float skillDamageIncreaseRate,
        string skillIcon, WeaponType requiredWeaponType, float skillDelay, float skillDelayDecreaseRate,
        float skillRange, float castingTime, int skillHitCount, int isActive, float buffDuration, float deBuffDuration, string prerequisiteSkillsId, float skillRadiusRange)
    {
        this.SkillId = skillId;
        this.SkillName = skillName;
        this.SkillDescription = skillDescription;
        this.SkillCooldown = skillCoolDown;
        this.SkillMaxLevel = skillMaxLevel;
        this.SkillDamage = skillDamage;
        this.SkillDamageIncreaseRate = skillDamageIncreaseRate;
        SetSkillIcon(skillIcon);
        this.RequiredWeapon = requiredWeaponType;
        this.UseSkillDelay = skillDelay;
        this.UseSkillDelayDecreaseRate = skillDelayDecreaseRate;
        this.SkillRange = skillRange;
        this.SkillCastingTime = castingTime;
        this.SkillHitCount = skillHitCount;
        SetSkillEffect(this.RequiredWeapon, this.SkillId, skillEffect);
        this.SkillRadiusRange = skillRadiusRange;
        this.BuffDuration = buffDuration;
        this.DeBuffDuration = deBuffDuration;
        SetPrerequisiteSkillsId(prerequisiteSkillsId);
        this.IsActive = isActive == 1;
    }
    
} 