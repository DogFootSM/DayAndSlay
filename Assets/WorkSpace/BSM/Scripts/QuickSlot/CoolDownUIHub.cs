using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CoolDownUIHub
{
    public static Dictionary<QuickSlotType, SkillCoolDown> CoolDownImageMap = new Dictionary<QuickSlotType, SkillCoolDown>();

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
        else
        {
            CoolDownImageMap[quickSlotType] = coolDownImage;
        }
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
