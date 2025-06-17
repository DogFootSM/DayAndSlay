using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotManager : MonoBehaviour
{
    [SerializeField] private QuickSlotUIManager quickSlotUIManager;
    [SerializeField] private QuickSlotSwap quickSlotSwap;
    [SerializeField] private QuickSlotRegister quickSlotRegister; 
    
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
    /// 슬롯 UI 매니저에 해당 스킬 노드로 이벤트 호출
    /// </summary>
    /// <param name="skillNode">선택한 스킬 노드</param>
    public void PreviewSkillInUI(SkillNode skillNode)
    { 
        quickSlotUIManager.OnSkillNodeSelected?.Invoke(skillNode);
    }

    /// <summary>
    /// 슬롯 UI 매니저의 스킬창 종료 시 프리뷰 종료
    /// </summary>
    public void ClosePreviewAndHideRegisterPanel()
    { 
        quickSlotUIManager.CloseSkillPreview();
        quickSlotUIManager.OnRequestRegisterPanelToggle?.Invoke(false); 
    }
 
    /// <summary>
    /// 스킬창 스킬 등록 판넬 활성화를 요청
    /// </summary>
    /// <param name="skillNode"></param>
    public void RequestSkillRegisterPanelOpen(SkillNode skillNode)
    {
        quickSlotRegister.OpenRegisterPanel(skillNode);
    }

    /// <summary>
    /// Quick Register에 현재 스킬창 스킬 등록을 진행
    /// </summary>
    /// <param name="quickSlotRegisterSlotUI">등록한 퀵슬롯 타입</param>
    /// <returns>퀵슬롯 UI에 사용할 스킬 노드 아이콘 이미지</returns>
    public SkillData RegisterSkillToQuickSlot(QuickSlotRegisterSlotUI quickSlotRegisterSlotUI)
    {  
        quickSlotRegister.ConfirmSkillRegister(quickSlotRegisterSlotUI); 
        return quickSlotRegister.SelectedSkillNode.skillData;
    }
 
    /// <summary>
    /// A, B 슬롯의 내용물 스왑 진행
    /// </summary>
    public void SwapSkillsInQuickSlots(QuickSlot prevQuickSlot, QuickSlot nextQuickSlot)
    {
        quickSlotSwap.SwapQuickSlotContents(prevQuickSlot, nextQuickSlot); 
    }
 
    
}
