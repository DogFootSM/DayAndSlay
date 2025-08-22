using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CoolDownUIHub
{
    public static Dictionary<QuickSlotType, SkillCoolDown> CoolDownImageMap = new Dictionary<QuickSlotType, SkillCoolDown>();

    public static void CoolDownUIRegistry(QuickSlotType quickSlotType, SkillCoolDown coolDownImage)
    {
        if (!CoolDownImageMap.ContainsKey(quickSlotType))
        {
            CoolDownImageMap.Add(quickSlotType, coolDownImage);
        } 
    }

    public static QuickSlotType SearchSkillCoolDown(SkillCoolDown skillCoolDown)
    {
        foreach (var findValue in CoolDownImageMap)
        {
            if (findValue.Value.Equals(skillCoolDown))
            {
                Debug.Log($"Ã£À½ :{findValue.Key}");
                return findValue.Key;
            }
        }

        return QuickSlotType.NONE;
    }
    
}
