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
    public int SkillLevel; 
    public int SkillMaxLevel;
    public int SkillDamage;
    public bool IsActive;
    
    public Sprite SkillIcon;
    public WeaponType RequiredWeapon;
    public SkillType SkillType;
    //TODO: 스킬 레벨은 db에 저장할거니까 얼마 찍었는지를 여기서 해줄 필요없이 Node에서 가지고 있는게 나으려나
    public GameObject SkillEffectPrefab;
    
    public List<string> prerequisiteSkillsId = new();
}
