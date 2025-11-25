using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CoolDownUIHub
{
    public static Dictionary<QuickSlotType, SkillCoolDown> CoolDownImageMap = new Dictionary<QuickSlotType, SkillCoolDown>();
    public static Dictionary<BuffType, BuffCoolDown> BuffCoolDownMap = new Dictionary<BuffType, BuffCoolDown>();
    
    /// <summary>
    /// 쿨타임 UI 객체 등록
    /// </summary>
    /// <param name="quickSlotType"></param>
    /// <param name="coolDownImage"></param>
    public static void CoolDownUIRegistry(QuickSlotType quickSlotType, SkillCoolDown coolDownImage)
    { 
        if (!CoolDownImageMap.ContainsKey(quickSlotType))
        {
            CoolDownImageMap.Add(quickSlotType, coolDownImage);
        }
        // else
        // {
        //     Debug.Log($"{quickSlotType} is already registered");
        //     CoolDownImageMap[quickSlotType] = coolDownImage;
        // }
    }

    /// <summary>
    /// 쿨다운 리셋 UI 등록
    /// </summary>
    /// <param name="buffType">사용한 버프 or 회피기 타입</param>
    /// <param name="coolDownImage"></param>
    public static void BuffCoolDownUIRegistry(BuffType buffType, BuffCoolDown coolDownImage)
    {
        BuffCoolDownMap.Add(buffType, coolDownImage);
    }
    
    /// <summary>
    /// 현재 쿨타임 진행중인 객체의 슬롯 타입을 찾은 후 반환
    /// </summary>
    /// <param name="skillCoolDown"></param>
    /// <returns></returns>
    public static QuickSlotType SearchSkillCoolDown(SkillCoolDown skillCoolDown)
    {
        foreach (var findValue in CoolDownImageMap)
        {
            if (findValue.Value.Equals(skillCoolDown))
            {
                return findValue.Key;
            }
        }

        return QuickSlotType.NONE;
    }
    
}
