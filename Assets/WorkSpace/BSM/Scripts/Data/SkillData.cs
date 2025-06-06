using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;

    public float SkillCooldown;
    public int SkillLevel;
    public int SkillExperience;
    public int SkillExperienceMax;
    public int SkillDamage;

    public Sprite SkillIcon;
    public WeaponType RequiredWeapon;
    public SkillType SkillType;

    public GameObject SkillEffectPrefab;
}
