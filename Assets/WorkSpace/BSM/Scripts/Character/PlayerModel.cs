using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

[Serializable]
public class PlayerStats
{
    //TODO: 스탯 공식 수정 필요, Critical float 타입으로 변경
    //레벨당 최대 경험치
    public int MaxExp => 100 + (int)(level * 10.5f);

    public int level;                    //캐릭터 레벨
    public int exp;                     //캐릭터 보유 경험치
    public float baseStrength;                //캐릭터 힘 능력치
    public float baseAgility;                 //캐릭터 민첩 능력치
    public float baseIntelligence;            //캐릭터 지능 능력치
    public float baseCriticalPer;                //캐릭터 크리티컬 능력치
    public int statsPoints;             //캐릭터 보유 스탯 포인트
    public int skillPoints;             //캐릭터 보유 스킬 포인트
    public float baseMoveSpeed;               //캐릭터의 기본 이동속도
    public float baseAttackSpeed;           //캐릭터의 기본 공격 속도
    public float baseCriticalDamage;
    public float baseCoolDown;
    public float baseCastingSpeed;
    public float baseResistance;
        
    public float EquipStrength;         //장비 장착 힘 능력치 
    public float EquipAgility;          //장비 장착 민첩 능력치
    public float EquipIntelligence;     //장비 장착 지능 능력치

    #region 패시브에 사용할 능력치
    public float StrengthFactor;        //기본 힘 * factor 만큼 증가한 능력치    
    public float AgilityFactor;         //기본 민첩 * factor 만큼 증가
    public float IntelligenceFactor;    //기본 지능 * factor 만큼 증가
    #endregion

    
    #region 상태창에서 보여지고 실질적으로 사용될 스탯 정보
    public float FinalStrength => baseStrength + EquipStrength + StrengthFactor;
    public float FinalAgility => baseAgility + EquipAgility + AgilityFactor;
    public float FinalIntelligence => baseIntelligence + EquipIntelligence + IntelligenceFactor;
    public float FinalCriticalPer;      
    public float FinalCriticalDamage;
    public float FinalResistance;
    
    public float IncreaseMoveSpeedPer;
    public float InCreaseAttackSpeedPer;
    #endregion
    
    //피해 반사
    public float DamageReflectRate;
    
    //패시브 스킬 방어력 증가 수치
    public float DefenseFactor;

    //방어력 관통 수치
    public float ArmorPenetration; 
    
    //TODO: 공격력 계산 공식 수정 필요
    //캐릭터 체력
    public int Health => level * (int)(FinalStrength * 0.2f);

    //캐릭터 물리 공격력
    public float PhysicalAttack => level * FinalStrength * 0.2f;

    //캐릭터 물리 방어력
    private float physicalDefense => level * (Health + FinalStrength) * 0.2f;
    public float PhysicalDefense => physicalDefense + (physicalDefense * DefenseFactor);
    
    //캐릭터 스킬 공격력
    public float SkillAttack => level * EquipStrength * FinalIntelligence * 0.2f;

    //캐릭터 스킬 방어력
    private float skillDefense => level * (Health + FinalIntelligence) * 0.2f;
    public float SkillDefense => skillDefense + (skillDefense * DefenseFactor);
    
    /// <summary>
    /// 아이템 수치에 따른 캐릭터 능력치 추가
    /// </summary>
    /// <param name="itemData">장착한 아이템 데이터</param>
    /// <param name="sign">1일 경우 장착, -1일 경우 장착 해제로 능력치 보정</param>
    public void AddStats(ItemData itemData, int sign)
    { 
        EquipStrength += itemData.Strength * sign;
        EquipAgility += itemData.Agility * sign;
        EquipIntelligence += itemData.Intelligence * sign;
    }
}

public class PlayerModel : MonoBehaviour, ISavable
{
    [Header("캐릭터 레벨업 시 스탯 포인트")] public int IncreaseStatsPoint;

    [Header("캐릭터 레벨업 시 스킬 포인트")] public int IncreaseSkillPoints;

    [Header("캐릭터 상태창")] [SerializeField] private StatusWindow statusWindow;

    [Header("캐릭터 스킬")] [SerializeField] private SkillTree skillTree;

    [SerializeField] private PlayerView playerView;
    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;
    [Inject] private SaveManager saveManager;

    private IDataReader dataReader;
    private GameManager gameManager => GameManager.Instance;
    private SoundManager soundManager => SoundManager.Instance;
    
    private PlayerStats playerStats;
    public PlayerStats PlayerStats => playerStats;
    
    
    /// <summary>
    /// 회피기 재사용 대기 시간
    /// </summary>
    private const float DODGE_COOLDOWN = 10f;
    public float DodgeCooldown => DODGE_COOLDOWN;
    
    /// <summary>
    /// 패링 재사용 대기 시간
    /// </summary>
    private const float PARRYING_COOLDOWN = 1f;
    public float ParryingCooldown => PARRYING_COOLDOWN;

    /// <summary>
    /// 텔레포트 재사용 대기 시간
    /// </summary>

    private const float BASE_TELEPORT_COOLDOWN = 10f;
    private float teleportCoolDown = BASE_TELEPORT_COOLDOWN;
    public float TeleportCoolDown => teleportCoolDown;
    
    
    public int CurSkillPoint
    {
        get => playerStats.skillPoints;

        set
        {
            playerStats.skillPoints += value;
            //스킬 포인트 변환시마다 스킬 레벨 증가 버튼 확인
            skillTree.NotifySkillPointChanged();
        }
    }

    private int slotId;

    private float maxHp;

    public float MaxHp
    {
        get => maxHp;
    }
    
    /// <summary>
    /// 현재 캐릭터 체력
    /// </summary>
    private float curHp;
    public float CurHp
    {
        get => curHp;
        set
        {
            curHp = value; 
            playerView.OnChangeHealth?.Invoke(curHp / maxHp, curHp, maxHp);
        }
    }

    /// <summary>
    /// 현재 남아있는 쉴드의 개수
    /// </summary>
    private int shieldCount;
    public int ShieldCount
    {
        get => shieldCount;
        set => shieldCount = value;
    }
    
    /// <summary>
    /// 방어력 증가 배수
    /// </summary>
    private float defenseBoostMultiplier;
    public float DefenseBoostMultiplier
    {
        get => defenseBoostMultiplier;
        set => defenseBoostMultiplier = value;
    }

    /// <summary>
    /// 현재 사용중인 스킬 캐스팅이 완료됐는지에 대한 상태
    /// </summary>
    private bool isCasting;
    public bool IsCasting
    {
        get => isCasting;
        set => isCasting = value;
    }
    
    /// <summary>
    /// 플레이어 움직임 가능 여부 상태
    /// </summary>
    private bool isMovementBlocked;
    public bool IsMovementBlocked
    {
        get => isMovementBlocked;
        set => isMovementBlocked = value;
    }

    /// <summary>
    /// 카운터 발동 상태 여부
    /// </summary>
    private bool isCountering;
    public bool IsCountering
    {
        get => isCountering;
        set => isCountering = value;
    }

    /// <summary>
    /// 다음 사용 스킬 데미지 증가 버프 적용 여부
    /// </summary>
    private bool nextSkillBuffActive;
    public bool NextSkillBuffActive
    {
        get => nextSkillBuffActive;
        set => nextSkillBuffActive = value;
    }

    /// <summary>
    /// 다음 사용 스킬 데미지 증가 배수
    /// </summary>
    private float nextSkillDamageMultiplier;
    public float NextSkillDamageMultiplier
    {
        get => nextSkillDamageMultiplier;
        set => nextSkillDamageMultiplier = value;
    }
    
    /// <summary>
    /// 데미지 감소 적용 비율
    /// </summary>
    private float damageReductionRatio;
    public float DamageReductionRatio
    {
        get => damageReductionRatio;
        set => damageReductionRatio = value;
    }
    
    public float FinalPhysicalDamage;
    public float FinalPhysicalDefense;
    public float MoveSpeed;
    public float AttackSpeed;

    //TODO: 사용 방법 생각
    public float CriticalPer;
    public float CriticalDamage;

    #region InGameManagerData
    private int gold;
    private int currentDay;
    private int debt;
    #endregion
    
    private float moveSpeedFactor;
    private float attackSpeedFactor;
    private float strengthFactor;
    private float criticalPerFactor;
    private float criticalDamageFactor;
    
    private CharacterWeaponType curWeaponType;
    public CharacterWeaponType ModelCurWeaponType => curWeaponType;

    private WeaponTierType curWeaponTier;
    private IngameManager ingameManagerInstance => IngameManager.instance;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        Init();
    }

    private void Init()
    {
        saveManager.SavableRegister(this);
        SetStatsData();
    }

    /// <summary>
    /// DB에서 읽어온 캐릭터 데이터 설정
    /// </summary>
    private void SetStatsData()
    {
        playerStats = new PlayerStats();

        //TODO: 테스트 용도로 슬롯ID 1으로 고정, 추후 제거하기
        slotId = 1;
        //slotId = dataManager.SlotId; 
        
        dataReader = sqlManager.ReadDataColumn(
            new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.EXP),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STATS_POINT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CHAR_LEVEL),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STRENGTH),
                sqlManager.GetCharacterColumn(CharacterDataColumns.AGILITY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.INTELLIGENCE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.SKILL_POINT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.GOLD),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CURRENTDAY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.DEBT)
            },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { $"{slotId}" },
            null);

        while (dataReader.Read())
        {
            playerStats.exp = dataReader.GetInt32(0);
            playerStats.statsPoints = dataReader.GetInt32(1);
            playerStats.level = dataReader.GetInt32(2);
            playerStats.baseStrength = dataReader.GetFloat(3);
            playerStats.baseAgility = dataReader.GetFloat(4);
            playerStats.baseIntelligence = dataReader.GetFloat(5);
            playerStats.skillPoints = dataReader.GetInt32(6);
            
            gold = dataReader.GetInt32(7);
            currentDay = dataReader.GetInt32(8);
            debt = dataReader.GetInt32(9);
            
            //기본값들로 변하지 않을 값
            playerStats.baseMoveSpeed = 3;
            playerStats.baseAttackSpeed = 0.5f;
            playerStats.IncreaseMoveSpeedPer = 1f;
            playerStats.InCreaseAttackSpeedPer = 1f;
            playerStats.baseCoolDown = 0f;
            playerStats.baseCastingSpeed = 0f;
            playerStats.baseResistance = 0f;
            playerStats.baseCriticalPer = 0f;
            playerStats.baseCriticalDamage = 1.5f;
        }
         
        //DB에서 불러온 Gold 정보 InGameManager에 설정
        ingameManagerInstance.SetGold(gold);
        
        //DB에서 불러온 Current Day정보 InGameManager에 설정
        ingameManagerInstance.SetCurrentDay(currentDay);
        
        //DB에서 불러온 Debt 정보 InGameManager에 설정
        ingameManagerInstance.SetDebt(debt);
         
        playerStats.FinalCriticalDamage = playerStats.baseCriticalDamage;
        MoveSpeed = GetFactoredMoveSpeed();
        AttackSpeed = GetFactorAttackSpeed();
    }

    private void Start()
    {
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        
        //경험치 바 초기화
        GainExperience(0);
    }

    private void Update()
    {
        //TODO: 테스트용 코드
        if (Input.GetKeyDown(KeyCode.V))
        { 
            GainExperience(500);
        } 
    }

    /// <summary>
    /// 현재 무기타입 업데이트
    /// </summary>
    /// <param name="weaponType">장착한 무기의 타입</param>
    public void UpdateWeaponType(CharacterWeaponType weaponType, WeaponTierType weaponTierType)
    {
        curWeaponType = weaponType;
        curWeaponTier = weaponTierType;
    }
    
    /// <summary>
    /// 이동 속도 증가값 설정 후 이동속도 업데이트
    /// </summary>
    /// <param name="speedFactor"></param>
    public void UpdateMoveSpeedFactor(float speedFactor)
    {
        //MoveSpeed에 증가할 값인 moveSpeedFactor 변수
        moveSpeedFactor = speedFactor;

        //증가된 값을 MoveSpeed에 적용
        MoveSpeed = GetFactoredMoveSpeed();
        
        playerStats.IncreaseMoveSpeedPer = 1f + moveSpeedFactor;
    }

    /// <summary>
    /// 이동속도 증가 값을 적용한 이동속도를 반환
    /// </summary>
    /// <returns></returns>
    public float GetFactoredMoveSpeed()
    {
        //기본 이동 속도에 factor만큼 증가한 값을 반환
        return playerStats.baseMoveSpeed + (moveSpeedFactor * playerStats.baseMoveSpeed);
    }
    
    /// <summary>
    /// 힘 증가 Factor 적용
    /// </summary>
    /// <param name="strengthFactor"></param>
    public void UpdateStrengthFactor(float strengthFactor)
    {
        playerStats.StrengthFactor = strengthFactor;
    }

    public void UpdateIntelligenceFactor(float intelligenceFactor)
    {
        playerStats.IntelligenceFactor = intelligenceFactor;
    }
    
    /// <summary>
    /// 현재 스피드에 Factor만큼 스피드 증가
    /// </summary>
    /// <param name="speedFactor"></param>
    public void UpdateAttackSpeedFactor(float speedFactor)
    {
        attackSpeedFactor = speedFactor;
        AttackSpeed = GetFactorAttackSpeed();
        playerStats.InCreaseAttackSpeedPer = 1f + speedFactor;
    }
    
    /// <summary>
    /// 현재 캐릭터의 공격 속도를 가져옴
    /// </summary>
    /// <returns></returns>
    private float GetFactorAttackSpeed()
    {
        return playerStats.baseAttackSpeed + (attackSpeedFactor * playerStats.baseAttackSpeed);
    }

    /// <summary>
    /// 현재 크리티컬 확률에 Factor만큼 증가
    /// </summary>
    /// <param name="criticalPerFactor"></param>
    public void UpdateCriticalPerFactor(float criticalPerFactor)
    {
        this.criticalPerFactor = criticalPerFactor;
        playerStats.FinalCriticalPer = GetFactorCriticalPerFactor();
    }
    
    /// <summary>
    /// 현재 크리티컬 확률 가져옴
    /// </summary>
    /// <returns></returns>
    public float GetFactorCriticalPerFactor()
    {
        return playerStats.baseCriticalPer + criticalPerFactor;
    }

    /// <summary>
    /// 현재 크리티컬 데미지 증가
    /// </summary>
    /// <param name="criticalDamageFactor">Factor만큼 CriticalDamage 값 증가</param>
    public void UpdateCriticalDamage(float criticalDamageFactor)
    {
        float round = Mathf.Round((playerStats.baseCriticalDamage + (playerStats.baseCriticalDamage * criticalDamageFactor)) * 100) * 0.01f;
        playerStats.FinalCriticalDamage = round;
    }
    
    /// <summary>
    /// 현재 상태이상에 Factor만큼 증가
    /// </summary>
    /// <param name="resistanceFactor"></param>
    public void UpdateResistanceFactor(float resistanceFactor)
    {
        playerStats.FinalResistance = playerStats.baseResistance + resistanceFactor;
    }

    /// <summary>
    /// 현재 방어력 Factor 업데이트
    /// 기본 방어력 Stats.PhysicalDefense 의 Factor 만큼 추가 스탯
    /// </summary>
    /// <param name="defenseFactor"></param>
    public void UpdateDefenseFactor(float defenseFactor)
    {
        playerStats.DefenseFactor = defenseFactor;
    }

    /// <summary>
    /// 방어구 관통력 스탯 업데이트
    /// </summary>
    /// <param name="armorPen"></param>
    public void UpdateArmorPenetration(float armorPen)
    {
        playerStats.ArmorPenetration = armorPen;
    }
    
    public void UpdateAgilityStats(float agility)
    {
       playerStats.AgilityFactor = agility;
    }
    
    //스킬 캐스팅 시간
    public float CastingTimeReduction { get; private set; } = 0f;
    
    /// <summary>
    /// 캐스팅 시간 계산
    /// </summary>
    /// <param name="value"></param>
    ///
    public void UpdateCastingTimeReduction(float value)
    {
        CastingTimeReduction = value;
    }
    
    /// <summary>
    /// 경험치 획득 후 레벨업 가능 여부 판단
    /// </summary>
    /// <param name="exp">Controller에서 전달 받을 경험치 수치</param>
    public void GainExperience(int exp)
    {
        playerStats.exp += exp;
        if (playerStats.exp >= playerStats.MaxExp)
        { 
            LevelUp();
        }

        playerView.OnChangeExp?.Invoke(playerStats.exp, playerStats.MaxExp);
    }
     
    /// <summary>
    /// 최종적으로 사용할 스탯의 값 업데이트
    /// </summary>
    private void UpdateFinalStats()
    {
        //최종적으로 사용할 물리 스탯
        FinalPhysicalDamage = playerStats.PhysicalAttack;
        FinalPhysicalDefense = playerStats.PhysicalDefense;

        //현재 체력, 최대 체력 업데이트
        curHp = playerStats.Health;
        maxHp = playerStats.Health;
        
        //체력 정보 UI 업데이트
        playerView.OnChangeHealth?.Invoke(curHp / maxHp, curHp, maxHp);
    }
 
    /// <summary>
    /// 캐릭터 레벨업 진행
    /// </summary>
    private void LevelUp()
    {
        soundManager.PlaySfx(SFXSound.LEVELUP);
        playerStats.level += GetExpToNextLevel();
        gameManager.HasUnsavedChanges = true;
        skillTree.NotifySkillPointChanged();
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        UpdateFinalStats();
    }

    /// <summary>
    /// 현재 획득한 경험치에 최대 경험치 계산 후 레벨 계산 및 스탯 포인트 획득
    /// </summary>
    /// <returns></returns>
    private int GetExpToNextLevel()
    {
        int addLevel = 0;
         
        //현재 경험치가 최대 경험치보다 클 경우
        while (playerStats.exp >= playerStats.MaxExp)
        {
            //현재 경험치에서 최대 경험치만큼 감소
            playerStats.exp -= playerStats.MaxExp;
            
            //레벨 1 증가
            addLevel++;
            
            //스탯 포인트 획득
            playerStats.statsPoints += IncreaseStatsPoint;
            playerStats.skillPoints += IncreaseSkillPoints;
        } 
        
        return addLevel;
    }
    
    /// <summary>
    /// 아이템 장착 시 스탯 효과 보정
    /// </summary> 
    public void ApplyItemModifiers(ItemData equipItemData, bool isEquip = true)
    {
        int sign = isEquip ? 1 : -1; 
        playerStats.AddStats(equipItemData, sign);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        UpdateFinalStats();
    }

    /// <summary>
    /// 패시브 능력 적용 후 스탯 정보 변경
    /// </summary>
    public void ApplyPassiveSkillModifiers()
    {
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
    }
    
    /// <summary>
    /// 스킬 포인트 사용 시 스탯 증가 기능
    /// </summary>
    /// <param name="statsType">증가할 스탯 타입 (힘, 민, 지) </param>
    /// <param name="increasePoint">기본 상승 수치</param>
    public void AdjustStats(CharacterStatsType statsType, int increasePoint = 1)
    {
        if (playerStats.statsPoints == 0) return;

        switch (statsType)
        {
            case CharacterStatsType.STR:
                playerStats.baseStrength += increasePoint;
                playerStats.EquipStrength += increasePoint;
                break;
            case CharacterStatsType.AGI:
                playerStats.baseAgility += increasePoint;
                playerStats.EquipAgility += increasePoint;
                break;
            case CharacterStatsType.INT:
                playerStats.baseIntelligence += increasePoint;
                playerStats.EquipIntelligence += increasePoint;
                break;
        }
        
        playerStats.statsPoints -= increasePoint;
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        UpdateFinalStats();
    }

    public void UpdateTeleportCoolDown(float factor)
    {
        teleportCoolDown = BASE_TELEPORT_COOLDOWN - (BASE_TELEPORT_COOLDOWN * factor);
    }
    
    /// <summary>
    /// PlayerStat 데이터 저장
    /// </summary>
    public bool Save(SqlManager sqlManager)
    { 
        return sqlManager.UpdateCharacterDataColumn(
            new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.EXP),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STATS_POINT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CHAR_LEVEL),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STRENGTH),
                sqlManager.GetCharacterColumn(CharacterDataColumns.AGILITY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.INTELLIGENCE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.SKILL_POINT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TIER),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CURRENTDAY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.DEBT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_SPRITE),
            },
            new[]
            {
                $"{playerStats.exp}",
                $"{playerStats.statsPoints}",
                $"{playerStats.level}",
                $"{playerStats.baseStrength}",
                $"{playerStats.baseAgility}",
                $"{playerStats.baseIntelligence}",
                $"{playerStats.skillPoints}",
                $"{(int)curWeaponType}",
                $"{(int)curWeaponTier}",
                $"{ingameManagerInstance.GetCurrentDay()}",
                $"{ingameManagerInstance.GetDebt()}",
                
                //TODO: 무기 저장 시 현재 빈손인지 확인하고 처리해서 저장하기
                $"{$"WEAPON_{curWeaponType.ToString()}_{curWeaponTier.ToString()}"}"
            },
            "slot_id",
            $"{dataManager.SlotId}"
        ); 
    }
}