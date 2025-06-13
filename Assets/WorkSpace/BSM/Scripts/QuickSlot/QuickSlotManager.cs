using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private SkillTreeUI skillTreeUI;
    [SerializeField] private GameObject registerPanel;
 
    private Dictionary<string, RegisterQuickSlot> registeredSkillMaps = new Dictionary<string, RegisterQuickSlot>();
    private Dictionary<QuickSlotType, RegisterQuickSlot> registerSlotsDict = new Dictionary<QuickSlotType, RegisterQuickSlot>();
    private Dictionary<QuickSlotType, QuickSlot> quickSlotsDict = new Dictionary<QuickSlotType, QuickSlot>();
    public static QuickSlotManager Instance;
    private SkillNode selectedSkillNode;
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
    }

    /// <summary>
    /// 선택한 스킬을 UI에 전달
    /// </summary>
    /// <param name="skillNode"></param>
    public void NotifySkillPreview(SkillNode skillNode)
    {
        skillTreeUI.UpdateSkillPreview(skillNode);
    }

    /// <summary>
    /// 스킬창 종료 시 프리뷰 종료
    /// </summary>
    public void CloseSkillPreview()
    {
        skillTreeUI.CloseSkillPreview();
        registerPanel.SetActive(false);
    }
 
    /// <summary>
    /// 퀵슬롯 등록 판넬 활성화
    /// </summary>
    /// <param name="skillNode"></param>
    public void SkillRegisterPanelOpen(SkillNode skillNode)
    {
        selectedSkillNode = skillNode;
        registerPanel.SetActive(true);
    }

    /// <summary>
    /// 퀵슬롯 스킬 등록 및 중복 스킬 제거
    /// </summary>
    /// <param name="registerQuickSlot">등록한 퀵슬롯 타입</param>
    /// <returns>퀵슬롯 UI에 사용할 스킬 노드 아이콘 이미지</returns>
    public SkillData PreviewSkillRegister(RegisterQuickSlot registerQuickSlot)
    {
        string key = selectedSkillNode.skillData.SkillId;
        
        UnassignDuplicateSkillSlot(key);
          
        registeredSkillMaps[key] = registerQuickSlot;
        registerPanel.SetActive(false);
        
        //메인 화면의 퀵슬롯에 등록할 스킬 노드 전달
        if (quickSlotsDict.ContainsKey(registerQuickSlot.QuickSlotType))
        {
            quickSlotsDict[registerQuickSlot.QuickSlotType].ApplySkillToMainQuickSlot(selectedSkillNode);
        }
        
        return selectedSkillNode.skillData;
    }

    /// <summary>
    /// 중복 스킬 슬롯 제거
    /// </summary>
    /// <param name="key"></param>
    private void UnassignDuplicateSkillSlot(string key)
    {
        //이미 등록된 스킬이면 슬롯 제거
        if (registeredSkillMaps.ContainsKey(key))
        {   
            QuickSlotType prevSlotType = registeredSkillMaps[key].QuickSlotType;
            registeredSkillMaps[key].PreviewUnRegisterSlot(); 
            quickSlotsDict[prevSlotType].UnassignExistingQuickSlotForSkill(); 
        }
    }
    
    /// <summary>
    /// A, B 슬롯의 내용물 교환
    /// </summary>
    public void SwapQuickSlotContents(QuickSlot prevQuickSlot, QuickSlot nextQuickSlot)
    {
        if (nextQuickSlot.CurrentSlotSkillNode == null)
        {
            nextQuickSlot.ApplySkillToMainQuickSlot(prevQuickSlot.CurrentSlotSkillNode);
            prevQuickSlot.UnassignExistingQuickSlotForSkill();
            
            registerSlotsDict[nextQuickSlot.CurrentQuickSlotType].UpdateRegisterSlot(nextQuickSlot.CurrentSlotSkillNode.skillData.SkillIcon);
            registerSlotsDict[prevQuickSlot.CurrentQuickSlotType].PreviewUnRegisterSlot();
            
            string prevKey = nextQuickSlot.CurrentSlotSkillNode.skillData.SkillId;
            registeredSkillMaps[prevKey] = registerSlotsDict[nextQuickSlot.CurrentQuickSlotType]; 
        }
        else
        {
             SkillNode prevSkillNode = prevQuickSlot.CurrentSlotSkillNode;
             SkillNode nextSkillNode = nextQuickSlot.CurrentSlotSkillNode;
            
             nextQuickSlot.ApplySkillToMainQuickSlot(prevSkillNode);
             prevQuickSlot.ApplySkillToMainQuickSlot(nextSkillNode);
             
             registerSlotsDict[nextQuickSlot.CurrentQuickSlotType].UpdateRegisterSlot(nextQuickSlot.CurrentSlotSkillNode.skillData.SkillIcon);
             registerSlotsDict[prevQuickSlot.CurrentQuickSlotType].UpdateRegisterSlot(prevQuickSlot.CurrentSlotSkillNode.skillData.SkillIcon);
             
             string key = nextQuickSlot.CurrentSlotSkillNode.skillData.SkillId;  
             registeredSkillMaps[key] = registerSlotsDict[nextQuickSlot.CurrentQuickSlotType]; 
             
             key = prevQuickSlot.CurrentSlotSkillNode.skillData.SkillId;
             registeredSkillMaps[key] = registerSlotsDict[prevQuickSlot.CurrentQuickSlotType]; 
             
        }
        
    } 

    /// <summary>
    /// 메인 화면 퀵슬롯 등록
    /// </summary>
    /// <param name="quickSlotType">퀵슬롯 타입</param>
    /// <param name="quickSlot">메인화면 퀵슬롯</param>
    public void AddQuickSlotEntry(QuickSlotType quickSlotType, QuickSlot quickSlot)
    {
        if (!quickSlotsDict.ContainsKey(quickSlotType))
        {
            quickSlotsDict.Add(quickSlotType, quickSlot);
        } 
    }

    public void AddRegisterQuickSlotEntry(QuickSlotType quickSlotType, RegisterQuickSlot quickSlot)
    {
        if (!registerSlotsDict.ContainsKey(quickSlotType))
        {
            registerSlotsDict.Add(quickSlotType, quickSlot);
        }
        
    } 
    
}
