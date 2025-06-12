using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;
 
[Serializable]
public class PlayerStats
{
    //TODO: 스탯 공식 수정 필요, Critical float 타입으로 변경
    //레벨당 최대 경험치
    public int MaxExp => 100 + (int)(level * 10.5f);
    
    public int level;               //캐릭터 레벨
    public int exp;                 //캐릭터 보유 경험치
    public int strength;            //캐릭터 힘 능력치
    public int agility;             //캐릭터 민첩 능력치
    public int intelligence;        //캐릭터 지능 능력치
    public int critical;            //캐릭터 크리티컬 능력치
    public int statsPoints;         //캐릭터 보유 스탯 포인트
    public int skillPoints;         //캐릭터 보유 스킬 포인트
    
    //캐릭터 체력
    public int Health => level * (int)(strength * 0.2f);
    
    //캐릭터 물리 공격력
    public int PhysicalAttack => level * (int)(strength * 0.2f);
    
    //캐릭터 물리 방어력
    public int PhysicalDefense => level * (int)((Health + strength) * 0.2f);
    
    //캐릭터 스킬 공격력
    public int SkillAttack => level * (int)(strength * intelligence * 0.2f);
    
    //캐릭터 스킬 방어력
    public int SkillDefense =>  level * (int)((Health + intelligence) * 0.2f);

    /// <summary>
    /// 아이템 수치에 따른 캐릭터 능력치 추가
    /// </summary>
    /// <param name="itemData">장착한 아이템 데이터</param>
    /// <param name="sign">1일 경우 장착, -1일 경우 장착 해제로 능력치 보정</param>
    public void AddStats(ItemData itemData, int sign)
    {
        strength += itemData.Strength * sign;
        agility += itemData.Agility * sign;
        intelligence += itemData.Intelligence * sign;
        critical += (int)itemData.Critical * sign; 
    }
    
}

public class PlayerModel : MonoBehaviour
{
    [Header("캐릭터 레벨업 시 스탯 포인트")]
    public int IncreaseStatsPoint;
    
    [Header("캐릭터 레벨업 시 스킬 포인트")]
    public int IncreaseSkillPoints;
    
    [Header("캐릭터 상태창")]
    [SerializeField] private StatusWindow statusWindow;
     
    [Header("캐릭터 스킬")]
    [SerializeField] private SkillTree skillTree; 
    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;
    private IDataReader dataReader;

    //이동 속도
    private float moveSpeed = 3f;
    public float MoveSpeed {get => moveSpeed;}
    
    private PlayerStats playerStats;
    
    //공격 스피드
    private float atkSpeed = 0.5f;
    public float AtkSpeed {get => atkSpeed;}
     
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
     
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        Init(); 
    }
  
    private void Init()
    { 
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
            new[] {sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID)},
            new []{$"{slotId}"},
            null);

        while (dataReader.Read())
        { 
            playerStats.exp = dataReader.GetInt32(0);
            playerStats.statsPoints = dataReader.GetInt32(1);
            playerStats.level = dataReader.GetInt32(2);
            playerStats.strength = dataReader.GetInt32(3);
            playerStats.agility = dataReader.GetInt32(4);
            playerStats.intelligence = dataReader.GetInt32(5);
            playerStats.intelligence = dataReader.GetInt32(6);
        }
    }
    
    private void Start()
    {
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GainExperience(50);
        }
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
            int remainExp = playerStats.exp - playerStats.MaxExp;
            
            LevelUp(remainExp);
        } 
    }

    /// <summary>
    /// 캐릭터 레벨업 진행
    /// </summary>
    /// <param name="remainExp">레벨업 후 남을 경험치</param>
    private void LevelUp(int remainExp)
    { 
        playerStats.exp = remainExp;
        playerStats.level++;
        playerStats.statsPoints += IncreaseStatsPoint;  
        playerStats.skillPoints += IncreaseSkillPoints;
        
        skillTree.NotifySkillPointChanged(); 
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
    }

    /// <summary>
    /// 아이템 장착 시 스탯 효과 보정
    /// </summary> 
    public void ApplyItemModifiers(ItemData equipItemData, bool isEquip = true)
    { 
        int sign = isEquip ? 1 : -1; 
        playerStats.AddStats(equipItemData, sign); 
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
                Debug.Log("힘 버튼 눌림");
                playerStats.strength += increasePoint;
                break;
            case CharacterStatsType.AGI:
                Debug.Log("민첩 버튼 눌림");
                playerStats.agility += increasePoint;
                break;
            case CharacterStatsType.INT:
                Debug.Log("지력 버튼 눌림");
                playerStats.intelligence += increasePoint;
                break;
        }
 
        playerStats.statsPoints -= increasePoint; 
        statusWindow.OnActiveIncreaseButton?.Invoke(playerStats.statsPoints);
        statusWindow.OnChangedAllStats?.Invoke(playerStats);
    }
}
