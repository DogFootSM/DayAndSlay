using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

public class SkillTree : MonoBehaviour, ISavable
{
    [SerializeField] private SkillTreePreview skillTreePreview;
    [SerializeField] private PlayerModel playerModel;
    [SerializeField] private PlayerSkillReceiver playerSkillReceiver;
    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;
    [Inject] private SaveManager saveManager;

    public List<SkillData> SkillDatas = new List<SkillData>();

    private SkillParticlePooling skillParticlePooling => SkillParticlePooling.Instance;
    private List<SkillNode> allskillNodes = new();
    private Dictionary<string, SkillNode> prerequisiteNodeMap = new();
    private Dictionary<WeaponType, List<SkillNode>> weaponTypeNodes = new();            //무기 타입별 노드
    private WeaponType curWeapon;
    private WeaponType beforeWeapon;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        InitializeSkillNodes();
        LinkPrerequisites();
        InitializeSkillData();
        CategorizeByWeapon();
        SortSkillNodesByWeapon();
        InstantiateSkillEffectInPool();
        saveManager.SavableRegister(this); 
    }

    /// <summary>
    /// 현재 스킬 레벨이 1 이상인 스킬 이펙트 스킬 풀링에 생성
    /// </summary>
    private void InstantiateSkillEffectInPool()
    {
        foreach (var skillNode in allskillNodes)
        {
            if (skillNode.CurSkillLevel > 0)
            {
                for (int i = 0; i < skillNode.skillData.SkillEffectPrefab.Count; i++)
                {
                    if(!skillNode.skillData.IsActive) continue;
                    skillParticlePooling.InstantiateSkillParticlePool($"{skillNode.skillData.SkillId}_{i + 1}_Particle", skillNode.skillData.SkillEffectPrefab[i]);
                }
                
                //skillParticlePooling.InstantiateSkillParticlePool(skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
            }
        }
    }
    
    /// <summary>
    /// 스킬 상태를 DB에서 가져와 설정
    /// </summary>
    private void InitializeSkillData()
    {
        IDataReader reader = sqlManager.ReadSkillDataColumn($"{dataManager.SlotId}");

        while (reader.Read())
        {
            string skillId = reader.GetString(0); //현재 스킬 ID
            int skillLevel = reader.GetInt32(1); //현재 스킬 레벨
            bool unlocked = reader.GetBoolean(2); //현재 스킬 해금

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
            allskillNodes.Add(new SkillNode(data, playerModel, playerSkillReceiver, (CharacterWeaponType)data.RequiredWeapon));
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
            foreach (string prerequisite in node.skillData.PrerequisiteSkillsId)
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
    /// 무기 타입별 스킬 아이디 값으로 정렬 후 각 스킬 탭 프리팹 생성
    /// </summary>
    private void SortSkillNodesByWeapon()
    {
        foreach (KeyValuePair<WeaponType, List<SkillNode>> node in weaponTypeNodes)
        {
            node.Value.Sort((x, y) => x.skillData.SkillId.CompareTo(y.skillData.SkillId));
        }

        //스킬 UI에 노드 리스트 전달
        skillTreePreview.InstantiateSkillPrefabs(weaponTypeNodes);
        NotifySkillPointChanged();
    }

    /// <summary>
    /// 스킬트리 UI에 현재 모델이 보유중인 스킬 포인트를 알림
    /// </summary>
    public void NotifySkillPointChanged()
    {
        skillTreePreview.UpdateAllNodeButtonsWithPoint(playerModel.CurSkillPoint);
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
        beforeWeapon = curWeapon;
        curWeapon = weaponType;
        skillTreePreview.OnChangedSkillTab?.Invoke(curWeapon);

        RemoveWeaponTypePassives();
        ApplyWeaponTypePassives(); 
    }
    
    
    /// <summary>
    /// 이전 무기의 패시브 적용 능력 제거
    /// </summary>
    private void RemoveWeaponTypePassives()
    {
        //이전 무기 패시브 능력 해제
        if (weaponTypeNodes.ContainsKey(beforeWeapon))
        {
            foreach (var skill in weaponTypeNodes[beforeWeapon])
            {
                PassiveSkill passiveSkill = SkillFactoryManager.GetSkillFactory(skill) as PassiveSkill;

                if (passiveSkill != null && skill.CurSkillLevel > 0)
                {
                    passiveSkill.RevertPassiveEffects();
                }
            }
        }
    }

    /// <summary>
    /// 현재 무기의 패시브 능력 적용
    /// </summary>
    private void ApplyWeaponTypePassives()
    {
        //현재 무기 패시브 적용
        if (weaponTypeNodes.ContainsKey(curWeapon))
        {
            foreach (var skill in weaponTypeNodes[curWeapon])
            {
                PassiveSkill passiveSkill = SkillFactoryManager.GetSkillFactory(skill) as PassiveSkill;

                if (passiveSkill != null && skill.CurSkillLevel > 0)
                { 
                    passiveSkill.ApplyPassiveEffects((CharacterWeaponType)curWeapon);
                } 
            }
        } 
    }
    
    /// <summary>
    /// 스킬 데이터 저장
    /// </summary>
    public bool Save(SqlManager sqlManager)
    {
        bool success = true;
        
        for (int i = 0; i < allskillNodes.Count; i++)
        {
            int lockState = allskillNodes[i].UnLocked ? 1 : 0;
            
            bool result = sqlManager.UpdateSkillDataColumn
            (
                new[] {"skill_level", "skill_unlocked"},
                new[] { $"{allskillNodes[i].CurSkillLevel}", $"{lockState}"},
                new[] { "slot_id", "skill_id"},
                new[] { $"{dataManager.SlotId}", allskillNodes[i].skillData.SkillId}
            );

            if (!result)
            {
                success = false;
                break;
            } 
        }
        
        return success;
    }

    /// <summary>
    /// 현재 무기 타입 노드에서 Skill_ID에 해당하는 스킬 노드를 반환
    /// </summary>
    /// <param name="weaponType">현재 무기 타입</param>
    /// <param name="skillId">찾으려는 스킬 데이터의 ID</param>
    /// <returns></returns>
    public SkillNode GetWeaponSkillNode(WeaponType weaponType, string skillId)
    {
        foreach (var skillNode in weaponTypeNodes[weaponType])
        {
            if (skillNode.skillData.SkillId.Equals(skillId))
            {
                return skillNode;
            }
        }
        
        return null;
    }
    
}