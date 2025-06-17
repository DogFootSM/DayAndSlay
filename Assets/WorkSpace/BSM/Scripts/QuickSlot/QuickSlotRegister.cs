using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotRegister : MonoBehaviour
{
    [SerializeField] private QuickSlotUIManager quickSlotUIManager;
    
    private SkillNode selectedSkillNode;
    public SkillNode SelectedSkillNode => selectedSkillNode;
    
    /// <summary>
    /// 퀵슬롯 스킬 등록 및 중복 스킬 제거
    /// </summary>
    /// <param name="quickSlotRegisterSlotUI">등록한 퀵슬롯 타입</param>
    /// <returns>퀵슬롯 UI에 사용할 스킬 노드 아이콘 이미지</returns>
    public SkillData ConfirmSkillRegister(QuickSlotRegisterSlotUI quickSlotRegisterSlotUI)
    {  
        //등록할 슬롯에 데이터가 존재하는지 확인
        if (quickSlotRegisterSlotUI.SkillData != null)
        { 
            //현재 슬롯의 데이터와 새로 선택한 스킬이 같은 스킬인지 비교
            if (!quickSlotRegisterSlotUI.SkillData.Equals(selectedSkillNode.skillData))
            { 
                //기존 슬롯에 할당되어 있는 스킬 제거
                string tempKey = quickSlotRegisterSlotUI.SkillData.SkillId;
                
               QuickSlotData.RegisteredSkillMaps[tempKey].PreviewUnRegisterSlot(); 
               QuickSlotData.QuickSlotsDict[quickSlotRegisterSlotUI.QuickSlotType].UnassignExistingQuickSlotForSkill(); 
               QuickSlotData.RegisteredSkillMaps.Remove(tempKey);
            } 
        }
        
        string key = selectedSkillNode.skillData.SkillId;
 
        UnassignDuplicateSkillSlot(key);
          
        QuickSlotData.RegisteredSkillMaps[key] = quickSlotRegisterSlotUI;
        quickSlotUIManager.OnRequestRegisterPanelToggle?.Invoke(false);
        
        //메인 화면의 퀵슬롯에 등록할 스킬 노드 전달
        if (QuickSlotData.QuickSlotsDict.ContainsKey(quickSlotRegisterSlotUI.QuickSlotType))
        {
            QuickSlotData.QuickSlotsDict[quickSlotRegisterSlotUI.QuickSlotType].ApplySkillToMainQuickSlot(selectedSkillNode);
        }
        
        return selectedSkillNode.skillData;
    }

    /// <summary>
    /// 중복 스킬 슬롯 제거
    /// </summary>
    /// <param name="key">스킬의 ID</param>
    private void UnassignDuplicateSkillSlot(string key)
    {
        //이미 등록된 같은 스킬이면 슬롯 제거
        if (QuickSlotData.RegisteredSkillMaps.ContainsKey(key))
        {  
            QuickSlotType prevSlotType = QuickSlotData.RegisteredSkillMaps[key].QuickSlotType;
            QuickSlotData.RegisteredSkillMaps[key].PreviewUnRegisterSlot(); 
            QuickSlotData.QuickSlotsDict[prevSlotType].UnassignExistingQuickSlotForSkill(); 
        }
    }

    public void OpenRegisterPanel(SkillNode skillNode)
    {
        selectedSkillNode = skillNode;
        quickSlotUIManager.OnRequestRegisterPanelToggle?.Invoke(true); 
    }
    
}
