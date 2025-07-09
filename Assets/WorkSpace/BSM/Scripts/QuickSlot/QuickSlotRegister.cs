using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuickSlotRegister : MonoBehaviour
{
    [SerializeField] private GameObject registerCanvas;

    [Header("스킬창 등록 퀵슬롯 부모")] [SerializeField]
    private GameObject previewSlotsParent;

    [Header("메인 화면 퀵슬롯 부모")] [SerializeField]
    private GameObject mainSlotParent;

    private SkillNode selectedSkillNode;                                            //스킬 등록창에서 선택한 스킬 노드
    private List<PreviewQuickSlot> previewQuickSlots = new();                       //스킬 등록창에 있는 퀵슬롯 요소들
    private List<MainQuickSlot> mainQuickSlots = new();                             //메인 화면에 노출되는 퀵슬롯 요소들

    private Dictionary<SkillNode, QuickSlotType> beforeQuickSlotTypeDict = new(); //중복 스킬 노드 할당을 확인하기 위한 변경 전 스킬 노드가 할당된 슬롯 타입

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
            if (!compareSkillNode.Equals(skillNode))
            { 
                if (beforeQuickSlotTypeDict.ContainsKey(compareSkillNode))
                {
                    //기존에 들어있던 스킬 제거
                    QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(quickSlotType);
                    beforeQuickSlotTypeDict.Remove(compareSkillNode); 
                }
                
                //슬롯에 덮어쓰기 전 기존 다른 슬롯에 할당되었는지 확인
                if (QuickSlotData.WeaponQuickSlotDict[weaponType].ContainsValue(skillNode))
                {
                    previewQuickSlots[(int)beforeQuickSlotTypeDict[skillNode]].SetPreviewSlot();
                    mainQuickSlots[(int)beforeQuickSlotTypeDict[skillNode]].SetMainQuickSlot();
                    QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(beforeQuickSlotTypeDict[skillNode]); 
                }
                
                beforeQuickSlotTypeDict[skillNode] = quickSlotType;
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode);
            } 
        }
        //해당 퀵슬롯에 스킬이 들어있지 않은 상태
        else
        {
            //다른 퀵슬롯에 해당 스킬이 할당되어 있는 상태
            if (beforeQuickSlotTypeDict.ContainsKey(skillNode))
            {
                QuickSlotData.WeaponQuickSlotDict[weaponType].Remove(beforeQuickSlotTypeDict[skillNode]);
                previewQuickSlots[(int)beforeQuickSlotTypeDict[skillNode]].SetPreviewSlot();
                mainQuickSlots[(int)beforeQuickSlotTypeDict[skillNode]].SetMainQuickSlot();
            
                beforeQuickSlotTypeDict[skillNode] = quickSlotType;
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode); 
            }
            else
            {
                QuickSlotData.WeaponQuickSlotDict[weaponType].TryAdd(quickSlotType, skillNode);
                beforeQuickSlotTypeDict.TryAdd(skillNode, quickSlotType);
                previewQuickSlots[(int)quickSlotType].SetPreviewSlot(skillNode);
                mainQuickSlots[(int)quickSlotType].SetMainQuickSlot(skillNode); 
            } 
        }
        
        
    }
}