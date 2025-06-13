using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotSwap : MonoBehaviour
{ 
    public void SwapQuickSlotContents(QuickSlot prevQuickSlot, QuickSlot nextQuickSlot)
    {
        //이동할 슬롯에 스킬 노드가 존재하는지 확인
        if (nextQuickSlot.CurrentSlotSkillNode == null)
        {
            nextQuickSlot.ApplySkillToMainQuickSlot(prevQuickSlot.CurrentSlotSkillNode);
            prevQuickSlot.UnassignExistingQuickSlotForSkill();
            
            QuickSlotData.RegisterSlotsDict[nextQuickSlot.CurrentQuickSlotType].UpdateRegisterSlot(nextQuickSlot.CurrentSlotSkillNode.skillData);
            QuickSlotData.RegisterSlotsDict[prevQuickSlot.CurrentQuickSlotType].PreviewUnRegisterSlot();

            QuickSlotData.UpdateSkillMaps(nextQuickSlot); 
        }
        else
        {
            SkillNode prevSkillNode = prevQuickSlot.CurrentSlotSkillNode;
            SkillNode nextSkillNode = nextQuickSlot.CurrentSlotSkillNode;
            
            UpdateSlotContents(prevSkillNode, nextQuickSlot);
            UpdateSlotContents(nextSkillNode, prevQuickSlot); 
        }
        
    }
    
    /// <summary>
    /// 변경 내용으로 슬롯 컨텐츠 업데이트
    /// </summary>
    /// <param name="skillNode">변경할 스킬 노드</param>
    /// <param name="quickSlot">변경될 퀵슬롯</param>
    private void UpdateSlotContents(SkillNode skillNode, QuickSlot quickSlot)
    {
        quickSlot.ApplySkillToMainQuickSlot(skillNode);
        QuickSlotData.RegisterSlotsDict[quickSlot.CurrentQuickSlotType].UpdateRegisterSlot(quickSlot.CurrentSlotSkillNode.skillData);
        QuickSlotData.UpdateSkillMaps(quickSlot);
    }
 
}
