using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSlotData
{
    public static Dictionary<string, QuickSlotRegisterSlotUI> RegisteredSkillMaps = new Dictionary<string, QuickSlotRegisterSlotUI>();
    public static Dictionary<QuickSlotType, QuickSlotRegisterSlotUI> RegisterSlotsDict = new Dictionary<QuickSlotType, QuickSlotRegisterSlotUI>();
    public static Dictionary<QuickSlotType, QuickSlot> QuickSlotsDict = new Dictionary<QuickSlotType, QuickSlot>();
    
    /// <summary>
    /// 등록된 스킬 맵 업데이트
    /// </summary>
    /// <param name="quickSlot">현재 퀵슬롯</param>
    public static void UpdateSkillMaps(QuickSlot quickSlot)
    {
        string key = quickSlot.CurrentSlotSkillNode.skillData.SkillId;    
        RegisteredSkillMaps[key] = RegisterSlotsDict[quickSlot.CurrentQuickSlotType];
    }
    
    /// <summary>
    /// 메인 화면 퀵슬롯 등록
    /// </summary>
    /// <param name="quickSlotType">퀵슬롯 타입</param>
    /// <param name="quickSlot">메인화면 퀵슬롯</param>
    public static void AddQuickSlotEntry(QuickSlotType quickSlotType, QuickSlot quickSlot)
    {
        if (!QuickSlotsDict.ContainsKey(quickSlotType))
        {
            QuickSlotsDict.Add(quickSlotType, quickSlot);
        } 
    }

    /// <summary>
    /// 스킬창 화면 퀵슬롯 등록
    /// </summary>
    /// <param name="quickSlotType"></param>
    /// <param name="slotUI"></param>
    public static void AddRegisterQuickSlotEntry(QuickSlotType quickSlotType, QuickSlotRegisterSlotUI slotUI)
    {
        if (!RegisterSlotsDict.ContainsKey(quickSlotType))
        {
            RegisterSlotsDict.Add(quickSlotType, slotUI);
        }
        
    }

    /// <summary>
    /// 현재 퀵슬롯에 등록되어 있는 스킬 노드 반환
    /// </summary>
    /// <param name="quickSlotType">퀵 슬롯 타입</param>
    /// <returns>사용한 퀵슬롯에 등록되어 있는 스킬 노드</returns>
    public static SkillNode GetQuickSlotSkillData(QuickSlotType quickSlotType)
    {
        if (QuickSlotsDict.TryGetValue(quickSlotType, out QuickSlot quickSlot))
        {
            if (quickSlot.CurrentSlotSkillNode != null && quickSlot.CurrentSlotSkillNode.skillData != null)
            { 
                return quickSlot.CurrentSlotSkillNode;
            }
        }
        
        return null;
    }
    
}
