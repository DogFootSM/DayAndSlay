using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuickSlotSetting
{
    public List<WeaponGroup> WeaponGroups = new();
}
 
[System.Serializable]
public class WeaponGroup
{
    public CharacterWeaponType WeaponType;
    public List<QuickSlotGroup> QuickSlotGroups = new List<QuickSlotGroup>();
}

[System.Serializable]
public class QuickSlotGroup
{
    public QuickSlotType QuickSlotType;
    public string SkillDataID; 
}