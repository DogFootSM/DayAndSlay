using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSlotData
{  
    public static Dictionary<CharacterWeaponType, Dictionary<QuickSlotType, SkillNode>> WeaponQuickSlotDict = new();
    public static Dictionary<SkillNode, QuickSlotType> BeforeQuickSlotTypeDict = new(); //중복 스킬 노드 할당을 확인하기 위한 변경 전 스킬 노드가 할당된 슬롯 타입

    /// <summary>
    /// 해당 퀵슬롯에 스킬이 할당되어 있는지 확인
    /// </summary>
    /// <param name="keyToQuickSlot">키보드로 입력한 퀵슬롯 타입</param>
    /// <param name="weaponType">현재 장착중인 무기의 타입</param>
    /// <returns></returns>
    public static bool IsSlotAssigned(QuickSlotType keyToQuickSlot, CharacterWeaponType weaponType)
    {
        return WeaponQuickSlotDict[weaponType].ContainsKey(keyToQuickSlot);
    }
}
