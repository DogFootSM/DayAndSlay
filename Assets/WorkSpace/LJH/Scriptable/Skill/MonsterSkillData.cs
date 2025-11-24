using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "MonsterSkillData", menuName = "Scriptable Object / MonsterSkillData")]
public class MonsterSkillData : ScriptableObject
{
    public string SkillName;
    [Header("분류")]
    public DamageType DamageType;
    public DelayType DelayType;

    [Header("스킬 정보")] 
    public float Damage;
    public float CoolDown;
    public float Delay;
    public float Duration;
    public float SkillMaxRange;
    public float SkillMinRange;

    [Header("시각 효과")]
    public ParticleSystem SkillEffect;
    public SpriteRenderer WarningEffect;
    public Collider2D AttackCollider;


}
