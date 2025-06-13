using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuickSlotUIManager : MonoBehaviour
{
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private SkillTreeUI skillTreeUI;
    
    public UnityAction<bool> OnRequestRegisterPanelToggle;
    public UnityAction<SkillNode> OnSkillNodeSelected;
    
    private void OnEnable()
    {
        OnRequestRegisterPanelToggle += SetRegisterPanelActive;
        OnSkillNodeSelected += UpdatePreviewUI;
    }

    private void OnDisable()
    {
        OnRequestRegisterPanelToggle -= SetRegisterPanelActive;
        OnSkillNodeSelected -= UpdatePreviewUI;
    }

    /// <summary>
    /// 퀵슬롯 등록 판넬 활성화 상태 변경
    /// </summary>
    /// <param name="isActive">On, Off 변경할 상태값</param>
    private void SetRegisterPanelActive(bool isActive)
    {
        registerPanel.SetActive(isActive);
    }

    /// <summary>
    /// 선택한 스킬을 UI에 전달
    /// </summary>
    /// <param name="skillNode">UI로 구성할 스킬 노드</param>
    private void UpdatePreviewUI(SkillNode skillNode)
    {
        skillTreeUI.UpdateSkillPreview(skillNode);
    }

    public void CloseSkillPreview()
    {
        skillTreeUI.CloseSkillPreview();
    }
    
}
