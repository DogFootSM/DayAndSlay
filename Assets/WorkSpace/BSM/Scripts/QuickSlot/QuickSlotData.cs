using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSlotData
{
    public static Dictionary<string, RegisterQuickSlot> RegisteredSkillMaps = new Dictionary<string, RegisterQuickSlot>();
    public static Dictionary<QuickSlotType, RegisterQuickSlot> RegisterSlotsDict = new Dictionary<QuickSlotType, RegisterQuickSlot>();
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
    /// <param name="quickSlot"></param>
    public static void AddRegisterQuickSlotEntry(QuickSlotType quickSlotType, RegisterQuickSlot quickSlot)
    {
        if (!RegisterSlotsDict.ContainsKey(quickSlotType))
        {
            RegisterSlotsDict.Add(quickSlotType, quickSlot);
        }
        
    } 
    
    
}
