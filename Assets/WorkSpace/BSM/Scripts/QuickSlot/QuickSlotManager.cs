using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private QuickSlotRegister quickSlotRegister;
    [SerializeField] private QuickSlotSwap quickSlotSwap;
    [SerializeField] private SkillTreePreview skillTreePreview;
    [SerializeField] private SkillTree skillTree;
    [Inject] private DataManager dataManager;
    
    public static QuickSlotManager Instance;
    private SkillNode selectedSkillNode;

    public SkillNode SelectedSkillNode
    {
        get => selectedSkillNode;
        set => selectedSkillNode = value;
    }

    public CharacterWeaponType CurrentWeaponType => curWeaponType;
    
    private CharacterWeaponType curWeaponType;
    private QuickSlotSetting quickSlotSetting;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } 
        ProjectContext.Instance.Container.Inject(this);
        Init();
    }

    private void Init()
    {
        //퀵슬롯 데이터 초기화
        foreach (CharacterWeaponType weaponType in Enum.GetValues(typeof(CharacterWeaponType)))
        {
            QuickSlotData.WeaponQuickSlotDict[weaponType] = new Dictionary<QuickSlotType, SkillNode>();
        }

        InitQuickSlotData();
    }

    /// <summary>
    /// 퀵슬롯 데이터 Load 시 초기화 작업
    /// </summary>
    private void InitQuickSlotData()
    {
        quickSlotSetting = dataManager.LoadQuickSlotSetting();

        //TODO: 퀵슬롯 데이터 lOAD
        //각 무기 그룹을 순회
        foreach (var weaponGroup in quickSlotSetting.WeaponGroups)
        {
            foreach (var slotGroup in weaponGroup.QuickSlotGroups)
            {
                //무기 타입과 퀵슬롯 타입 데이터를 가져와 스킬 데이터 할당
                QuickSlotData.WeaponQuickSlotDict[weaponGroup.WeaponType][slotGroup.QuickSlotType] =
                    skillTree.GetWeaponSkillNode((WeaponType)weaponGroup.WeaponType, slotGroup.SkillDataID);
                
                //현재 등록한 스킬에 따른 퀵슬롯 맵핑
                QuickSlotData.BeforeQuickSlotTypeDict[
                        skillTree.GetWeaponSkillNode((WeaponType)weaponGroup.WeaponType, slotGroup.SkillDataID)] =
                    slotGroup.QuickSlotType;
            }
        } 
    }
    
    /// <summary>
    /// 현재 무기 타입 업데이트
    /// </summary>
    /// <param name="weaponType"></param>
    public void UpdateWeaponType(CharacterWeaponType weaponType)
    {
        curWeaponType = weaponType;
        quickSlotRegister.UpdateQuickSlotsByWeaponChange(weaponType);
    }
    
    /// <summary>
    /// 스킬 설명 등 선택한 스킬 상세 정보 UI 업데이트
    /// </summary>
    public void UpdateSkillPreviewTab(SkillNode skillNode)
    {
        skillTreePreview.UpdateSkillPreview(skillNode); 
    }
    
    /// <summary>
    /// 퀵슬롯 등록 캔버스 활성화 요청
    /// </summary>
    public void OpenRegisterCanvas()
    {
        quickSlotRegister.OpenRegisterCanvas();
    }

    /// <summary>
    /// 선택한 스킬 등록 진행
    /// </summary>
    /// <returns></returns>
    public void RegisteredSkillNode(QuickSlotType quickSlotType)
    {
        quickSlotRegister.RegisterSkillNode(curWeaponType, quickSlotType, selectedSkillNode);
    }

    /// <summary>
    /// 메인 화면에서 인식한 퀵슬롯들의 스왑을 요청
    /// </summary>
    /// <param name="beginSlot"></param>
    /// <param name="endSlot"></param>
    public void SlotSwapRequest(QuickSlotType beginSlot, QuickSlotType endSlot)
    {
        quickSlotSwap.QuickSlotSkillNodeSwap(beginSlot, endSlot, curWeaponType);
        quickSlotRegister.UpdateSlotUI(beginSlot, endSlot, curWeaponType); 
        SkillCoolDownUIChange(beginSlot, endSlot);
        SkillWaitUseUIChange(beginSlot, endSlot);
    }

    private void SkillWaitUseUIChange(QuickSlotType beginSlot, QuickSlotType endSlot)
    {
        GameObject updateBeginParent = QuickSlotWaitUseUI.QuickSlotWaitUses[endSlot].transform.parent.gameObject;
        GameObject updateEndParent = QuickSlotWaitUseUI.QuickSlotWaitUses[beginSlot].transform.parent.gameObject;
        
        QuickSlotWaitUseUI.QuickSlotWaitUses[beginSlot].transform.SetParent(updateBeginParent.transform);
        QuickSlotWaitUseUI.QuickSlotWaitUses[endSlot].transform.SetParent(updateEndParent.transform);

        QuickSlotWaitUse temp = QuickSlotWaitUseUI.QuickSlotWaitUses[beginSlot];
        QuickSlotWaitUseUI.QuickSlotWaitUses[beginSlot] = QuickSlotWaitUseUI.QuickSlotWaitUses[endSlot];
        QuickSlotWaitUseUI.QuickSlotWaitUses[endSlot] = temp;
        
        QuickSlotWaitUseUI.QuickSlotWaitUses[beginSlot].UpdateRectTransform();
        QuickSlotWaitUseUI.QuickSlotWaitUses[endSlot].UpdateRectTransform();
    }
    
    private void SkillCoolDownUIChange(QuickSlotType beginSlot, QuickSlotType endSlot)
    {
        GameObject updateBeginParent = CoolDownUIHub.CoolDownImageMap[endSlot].transform.parent.gameObject;
        GameObject updateEndParent = CoolDownUIHub.CoolDownImageMap[beginSlot].transform.parent.gameObject;
        
        CoolDownUIHub.CoolDownImageMap[beginSlot].transform.SetParent(updateBeginParent.transform);
        CoolDownUIHub.CoolDownImageMap[endSlot].transform.SetParent(updateEndParent.transform);

        SkillCoolDown temp = CoolDownUIHub.CoolDownImageMap[beginSlot];
        CoolDownUIHub.CoolDownImageMap[beginSlot] = CoolDownUIHub.CoolDownImageMap[endSlot];
        CoolDownUIHub.CoolDownImageMap[endSlot] = temp;
        
        CoolDownUIHub.CoolDownImageMap[beginSlot].UpdateAnchorPreset();
        CoolDownUIHub.CoolDownImageMap[endSlot].UpdateAnchorPreset();
    }
    
}
