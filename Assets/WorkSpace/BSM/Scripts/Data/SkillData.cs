using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    public string SkillId;
    public string SkillName;
    public string SkillDescription;

    public float SkillCooldown;
    public float RecoveryTime;
    public int SkillMaxLevel;
    public int SkillDamage;
    public bool IsActive;
    
    public Sprite SkillIcon;
    public WeaponType RequiredWeapon;
    public SkillType SkillType;

    public GameObject SkillEffectPrefab;

    public List<string> prerequisiteSkillsId = new();
} 