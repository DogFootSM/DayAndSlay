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
 
    private Dictionary<string, RegisterQuickSlot> registeredSkills = new Dictionary<string, RegisterQuickSlot>();
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
    /// 퀵슬롯 스킬 등록
    /// </summary>
    /// <param name="registerQuickSlot">등록한 퀵슬롯 타입</param>
    /// <returns>퀵슬롯 UI에 사용할 스킬 노드 데이터</returns>
    public SkillNode PreviewSkillRegister(RegisterQuickSlot registerQuickSlot)
    {
        string key = selectedSkillNode.skillData.SkillId;
        
        //이미 등록된 스킬이면 슬롯 제거
        if (registeredSkills.ContainsKey(key))
        {
            registeredSkills[key].PreviewUnRegisterSlot();
            registeredSkills[key] = null;
        }
        
        registeredSkills[key] = registerQuickSlot;
        registerPanel.SetActive(false);
        
        return selectedSkillNode;
    }
    
}
