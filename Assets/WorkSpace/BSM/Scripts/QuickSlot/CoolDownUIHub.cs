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
    
}
