using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSlotWaitUseUI
{
     public static Dictionary<QuickSlotType, QuickSlotWaitUse> QuickSlotWaitUses = new Dictionary<QuickSlotType, QuickSlotWaitUse>();

     public static void WaitUseRegistry(QuickSlotType quickSlotType, QuickSlotWaitUse quickSlotWaitUse)
     {
          if (!QuickSlotWaitUses.ContainsKey(quickSlotType))
          {
               QuickSlotWaitUses.Add(quickSlotType, quickSlotWaitUse);
          }
     }
}
