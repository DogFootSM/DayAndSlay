using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuickSlotRegister : MonoBehaviour
{
    [SerializeField] private GameObject registerCanvas;
    [SerializeField] private QuickSlotToastMessage quickSlotToast;
    
    [Header("스킬창 등록 퀵슬롯 부모")] [SerializeField]
    private GameObject previewSlotsParent;

    [Header("메인 화면 퀵슬롯 부모")] [SerializeField]
    private GameObject mainSlotParent;

    private SkillNode selectedSkillNode;                                            //스킬 등록창에서 선택한 스킬 노드
    private List<PreviewQuickSlot> previewQuickSlots = new();                       //스킬 등록창에 있는 퀵슬롯 요소들
    private List<MainQuickSlot> mainQuickSlots = new();                             //메인 화면에 노출되는 퀵슬롯 요소들

    

    private void Awake()
    {
        //스킬 등록 프리뷰 슬롯
        for (int i = 0; i < previewSlotsParent.transform.childCount; i++)
        {
            previewQuickSlots.Add(previewSlotsParent.transform.GetChild(i).GetComponent<PreviewQuickSlot>());
        }

        //메인 화면 퀵슬롯
        for (int i = 0; i < mainSlotParent.transform.childCount; i++)
        {
            mainQuickSlots.Add(mainSlotParent.transform.GetChild(i).GetComponent<MainQuickSlot>());
        }
    }
 
    /// <summary>
    /// 퀵슬롯 등록할 캔버스 활성화
    /// </summary>
    public void OpenRegisterCanvas()
    {
        registerCanvas.SetActive(true);
    }
     
    /// <summary>
    /// 장착 무기에 따른 스킬 퀵슬롯 등록
    /// </summary>
    /// <param name="weaponType"></param>
    /// <param name="quickSlotType"></param>
    /// <param name="skillNode"></param>
    public void RegisterSkillNode(CharacterWeaponType weaponType, QuickSlotType quickSlotType, SkillNode skillNode)
    {
        //해당 퀵슬롯에 스킬이 들어있는 상태
        if (QuickSlotData.WeaponQuickSlotDict[weaponType].TryGetValue(quickSlotType, out SkillNode compareSkillNode))
        {
            if (!compareSkillNode.IsCoolDownReset)
            {
                quickSlotToast.ShowToast();
                return;
            }
            
            if (!compareSkillNode.Equals(skillNode))
            { 
                if (QuickSlotData.BeforeQuickSlotTypeDict.ContainsKey(compareSkillNode))
                {
                    //기존에 들어있던 스킬 제거
                    QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(quickSlotType);
                    QuickSlotData.BeforeQuickSlotTypeDict.Remove(compareSkillNode); 
                    CoolDownUIHub.CoolDownImageMap[quickSlotType].IsCooldownRunning();
                }
                
                //슬롯에 덮어쓰기 전 기존 다른 슬롯에 할당되었는지 확인
                if (QuickSlotData.WeaponQuickSlotDict[weaponType].ContainsValue(skillNode))
                {
                    previewQuickSlots[(int)QuickSlotData.BeforeQuickSlotTypeDict[skillNode]].SetPreviewSlot();
                    mainQuickSlots[(int)QuickSlotData.BeforeQuickSlotTypeDict[skillNode]].SetMainQuickSlot();
                    QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(QuickSlotData.BeforeQuickSlotTypeDict[skillNode]); 
                }
                
                QuickSlotData.BeforeQuickSlotTypeDict[skillNode] = quickSlotType;
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode);
            } 
        }
        //해당 퀵슬롯에 스킬이 들어있지 않은 상태
        else
        {
            //다른 퀵슬롯에 해당 스킬이 할당되어 있는 상태
            if (QuickSlotData.BeforeQuickSlotTypeDict.ContainsKey(skillNode))
            {
                if (!skillNode.IsCoolDownReset)
                {
                    quickSlotToast.ShowToast();
                    return;
                }
                
                QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(QuickSlotData.BeforeQuickSlotTypeDict[skillNode]);
                previewQuickSlots[(int)QuickSlotData.BeforeQuickSlotTypeDict[skillNode]].SetPreviewSlot();
                mainQuickSlots[(int)QuickSlotData.BeforeQuickSlotTypeDict[skillNode]].SetMainQuickSlot();
            
                QuickSlotData.BeforeQuickSlotTypeDict[skillNode] = quickSlotType;
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode); 
            }
            else
            {
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                QuickSlotData.BeforeQuickSlotTypeDict.TryAdd(skillNode, quickSlotType);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode); 
            } 
        } 
        
        registerCanvas.SetActive(false);
    }

    /// <summary>
    /// 무기 변경, 데이터 로드 시 메인 화면과 스킬 창 등록 화면의 퀵슬롯 데이터 설정
    /// </summary>
    /// <param name="weaponType">현재 착용중인 무기타입</param>
    public void UpdateQuickSlotsByWeaponChange(CharacterWeaponType weaponType)
    {
        for (var i = QuickSlotType.Q; i < QuickSlotType.NONE; i++)
        {
            //퀵슬롯 타입이 등록된 상태
            if (QuickSlotData.WeaponQuickSlotDict[weaponType].ContainsKey(i))
            {
                mainQuickSlots[(int)i].SetMainQuickSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][i]); 
                previewQuickSlots[(int)i].SetPreviewSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][i]);
            }
            else
            {
                //등록되지 않은 퀵 슬롯 초기화
                mainQuickSlots[(int)i].SetMainQuickSlot();
                previewQuickSlots[(int)i].SetPreviewSlot();
            } 
        } 
    }

    /// <summary>
    /// 퀵슬롯 스왑 후 새로 할당된 스킬 노드로 갱신
    /// </summary>
    /// <param name="beginSlot"></param>
    /// <param name="endSlot"></param>
    /// <param name="weaponType"></param>
    public void UpdateSlotUI(QuickSlotType beginSlot, QuickSlotType endSlot, CharacterWeaponType weaponType)
    {
        //스킬이 할당된 퀵슬롯과 빈슬롯 교환
        if (!QuickSlotData.WeaponQuickSlotDict[weaponType].ContainsKey(beginSlot))
        {
            previewQuickSlots[(int)beginSlot].SetPreviewSlot();
            mainQuickSlots[(int)beginSlot].SetMainQuickSlot();
            
            previewQuickSlots[(int)endSlot].SetPreviewSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot]);
            mainQuickSlots[(int)endSlot].SetMainQuickSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot]);
        }
        //스킬이 할당된 두 슬롯의 교환
        else
        {
            previewQuickSlots[(int)endSlot].SetPreviewSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot]);
            mainQuickSlots[(int)endSlot].SetMainQuickSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][endSlot]);
            
            previewQuickSlots[(int)beginSlot].SetPreviewSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot]);
            mainQuickSlots[(int)beginSlot].SetMainQuickSlot(QuickSlotData.WeaponQuickSlotDict[weaponType][beginSlot]);
            
        }
    }
}