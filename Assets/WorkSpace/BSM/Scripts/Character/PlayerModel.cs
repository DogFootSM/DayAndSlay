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
    public float FinalCriticalPer;      //보여질 캐릭터 크리티컬 능력치
    public float FinalCriticalDamage;
    public float FinalResistance;
    
    public float IncreaseMoveSpeedPer;
    public float InCreaseAttackSpeedPer;
    #endregion

    public float baseCoolDown;
    public float baseCastingSpeed;
    public float baseResistance;
    public float DamageReflectRate;
    
    public float DefenseFactor;                 //패시브 스킬 방어력 증가 수치
    
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
    
    private PlayerStats playerStats;
    public PlayerStats PlayerStats => playerStats;
    
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
    public float CriticalPer;
    public float CriticalDamage;
    
    private float moveSpeedFactor;
    private float attackSpeedFactor;
    private float strengthFactor;
    private float criticalPerFactor;
    private float criticalDamageFactor;
    private float armorPenetration;
    
    
    private CharacterWeaponType curWeaponType;
    public CharacterWeaponType ModelCurWeaponType => curWeaponType;
    
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
                sqlManager.GetCharacterColumn(CharacterDataColumns.SKILL_POINT)
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

        MoveSpeed = GetFactoredMoveSpeed();
        AttackSpeed = GetFactorAttackSpeed();
    }

    private void Start()
    {
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
    }

    private void Update()
    {
        //TODO: 테스트용 코드
        if (Input.GetKeyDown(KeyCode.V))
        { 
            GainExperience(50);
        } 
    }

    /// <summary>
    /// 현재 무기타입 업데이트
    /// </summary>
    /// <param name="weaponType">장착한 무기의 타입</param>
    public void UpdateWeaponType(CharacterWeaponType weaponType)
    {
        curWeaponType = weaponType;
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
        return playerStats.baseMoveSpeed + moveSpeedFactor;
    }
    
    /// <summary>
    /// 힘 증가 Factor 적용
    /// </summary>
    /// <param name="strengthFactor"></param>
    public void UpdateStrengthFactor(float strengthFactor)
    {
        playerStats.StrengthFactor = strengthFactor;
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
    /// 현재 캐릭터의 이동 속도를 가져옴
    /// </summary>
    /// <returns></returns>
    public float GetFactorAttackSpeed()
    {
        return playerStats.baseAttackSpeed + attackSpeedFactor;
    }

    /// <summary>
    /// 현재 크리티컬 데미지에 Factor만큼 증가
    /// </summary>
    /// <param name="criticalPerFactor"></param>
    public void UpdateCriticalPerFactor(float criticalPerFactor)
    {
        this.criticalPerFactor = criticalPerFactor;
        playerStats.FinalCriticalPer = GetFactorCriticalPerFactor();
    }
    
    /// <summary>
    /// 현재 크리티컬 데미지를 가져옴
    /// </summary>
    /// <returns></returns>
    public float GetFactorCriticalPerFactor()
    {
        return playerStats.baseCriticalPer + criticalPerFactor;
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
        armorPenetration = armorPen;
    }
    
    private void UpdateAgilityStats(float agility)
    {
       
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
        playerStats.level += playerStats.exp / playerStats.MaxExp;              //TODO: 경험치 몫에 대한 재귀처리로 레벨업 필요할 것으로 보임, Max 경험치에 대한 갱신
        playerStats.exp %= playerStats.MaxExp;
        playerStats.statsPoints += IncreaseStatsPoint;
        playerStats.skillPoints += IncreaseSkillPoints;
        gameManager.HasUnsavedChanges = true;
        skillTree.NotifySkillPointChanged();
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        UpdateFinalStats();
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
    
    
    
    /// <summary>
    /// PlayerStat 데이터 저장
    /// </summary>
    public void Save(SqlManager sqlManager)
    { 
        sqlManager.UpdateCharacterDataColumn(
            new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.EXP),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STATS_POINT),
                sqlManager.GetCharacterColumn(CharacterDataColumns.CHAR_LEVEL),
                sqlManager.GetCharacterColumn(CharacterDataColumns.STRENGTH),
                sqlManager.GetCharacterColumn(CharacterDataColumns.AGILITY),
                sqlManager.GetCharacterColumn(CharacterDataColumns.INTELLIGENCE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.SKILL_POINT)
            },
            new[]
            {
                $"{playerStats.exp}",
                $"{playerStats.statsPoints}",
                $"{playerStats.level}",
                $"{playerStats.baseStrength}",
                $"{playerStats.baseAgility}",
                $"{playerStats.baseIntelligence}",
                $"{playerStats.skillPoints}"
            },
            "slot_id",
            $"{dataManager.SlotId}"
        );


        Debug.Log("스탯 저장 진행");
    }
}