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


    public void SetVfx(List<GameObject> packages)
    {
        SkillEffect = packages[0].GetComponent<ParticleSystem>();
        WarningEffect = packages[1].GetComponent<SpriteRenderer>();
        AttackCollider =  packages[2].GetComponent<Collider2D>();
    }

    /// <summary>
    /// 스킬의 범위
    /// </summary>
    /// <param name="targetPos">이펙트가 표시되어야 할 pos</param>
    public void SetSkillRadius(Vector3 targetPos)
    {
        if (SkillEffect != null)
        {
            SkillEffect.transform.position = targetPos;
        }

        if (WarningEffect != null)
        {
            WarningEffect.transform.position = targetPos;
        }

        if (AttackCollider != null)
        {
            AttackCollider.transform.position = targetPos;
        }
    }
}
