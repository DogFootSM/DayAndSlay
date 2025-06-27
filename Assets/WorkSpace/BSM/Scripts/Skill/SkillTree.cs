using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private SkillTreeUI skillTreeUI;
    [SerializeField] private PlayerModel playerModel;
    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;
    [Inject] private SaveManager saveManager;
    
    public List<SkillData> SkillDatas = new List<SkillData>();

    private List<SkillNode> allskillNodes = new();

    private Dictionary<string, SkillNode> prerequisiteNodeMap = new();
    private Dictionary<WeaponType, List<SkillNode>> weaponTypeNodes = new();
    private WeaponType curWeapon;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        InitializeSkillNodes();
        LinkPrerequisites();
        InitializeSkillData();
        CategorizeByWeapon();
        SortSkillNodesByWeapon();
        
        saveManager.InitSkillNode(this);
    }
 
    /// <summary>
    /// 스킬 상태를 DB에서 가져와 설정
    /// </summary>
    private void InitializeSkillData()
    {
        //IDataReader reader = sqlManager.ReadSkillDataColumn($"{dataManager.SlotId}");
        //TODO: 테스트용 슬롯 아이디 1고정
        IDataReader reader = sqlManager.ReadSkillDataColumn("1");

        while (reader.Read())
        {
            string skillId = reader.GetString(0);           //현재 스킬 ID
            int skillLevel = reader.GetInt32(1);            //현재 스킬 레벨
            bool unlocked = reader.GetBoolean(2);           //현재 스킬 해금

            //스킬 ID 키가 있을 경우 스킬 DB 정보 반영
            if (prerequisiteNodeMap.ContainsKey(skillId))
            {
                prerequisiteNodeMap[skillId].LoadSkillFromDB(skillLevel, unlocked);
            } 
        } 
    }

    /// <summary>
    /// 각 스킬 노드 생성
    /// </summary>
    private void InitializeSkillNodes()
    {
        foreach (SkillData data in SkillDatas)
        {
            //노드 생성 후 전체 스킬 노드 리스트에 추가
            allskillNodes.Add(new SkillNode(data, playerModel));
        }
    }

    /// <summary>
    /// 선행 스킬 노드 연결
    /// </summary>
    private void LinkPrerequisites()
    {
        //전체 스킬 리스트에서 각 스킬 이름 별로 딕셔너리 맵핑
        prerequisiteNodeMap = allskillNodes.ToDictionary(x => x.skillData.SkillId);

        foreach (SkillNode node in allskillNodes)
        { 
            //해당 노드의 선행 스킬 리스트 이름 추출
            foreach (string prerequisite in node.skillData.prerequisiteSkillsId)
            {
                //딕셔너리에 해당 이름의 노드를 찾아서 선행 스킬 노드 연결
                if (prerequisiteNodeMap.TryGetValue(prerequisite, out SkillNode skillNode))
                {
                    node.AddPrerequisiteSkillNode(skillNode);
                }
            }
        }
    }

    /// <summary>
    /// 노드 내 데이터 오름차순 정렬
    /// </summary>
    private void SortSkillNodesByWeapon()
    {
        foreach (KeyValuePair<WeaponType, List<SkillNode>> node in weaponTypeNodes)
        {
            node.Value.Sort((x, y) => x.skillData.SkillId.CompareTo(y.skillData.SkillId));
        }
        
        //스킬 UI에 노드 리스트 전달
        skillTreeUI.InstantiateSkillPrefabs(weaponTypeNodes); 
        NotifySkillPointChanged();
    }

    /// <summary>
    /// 스킬트리 UI에 현재 모델이 보유중인 스킬 포인트를 알림
    /// </summary>
    public void NotifySkillPointChanged()
    { 
        skillTreeUI.UpdateAllNodeButtonsWithPoint(playerModel.CurSkillPoint);
    }
    
    /// <summary>
    /// 무기 타입에 따른 노드 구분
    /// </summary>
    private void CategorizeByWeapon()
    {
        foreach (SkillNode skillNode in allskillNodes)
        {
            WeaponType skillWeaponKey = skillNode.skillData.RequiredWeapon;

            if (!weaponTypeNodes.ContainsKey(skillWeaponKey))
            {
                weaponTypeNodes[skillWeaponKey] = new List<SkillNode>();
            }

            weaponTypeNodes[skillWeaponKey].Add(skillNode);
        }
    }

    /// <summary>
    /// 무기에 따른 현재 무기 타입 변경
    /// </summary>
    /// <param name="weaponType">현재 무기 타입</param>
    public void ChangedWeaponType(WeaponType weaponType)
    {
        curWeapon = weaponType;
        skillTreeUI.OnChangedSkillTab?.Invoke(curWeapon);
    }

    
}