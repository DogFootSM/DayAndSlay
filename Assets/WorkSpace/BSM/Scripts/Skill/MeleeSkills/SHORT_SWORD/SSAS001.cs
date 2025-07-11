using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS001 : MeleeSkill
{
    private float leftDeg = 90f; 
    private float rightDeg = 270f;
    private float downDeg = 180f;
    private float upDeg = 0f;

    private Vector2 dir;
    private Vector2 pos;

    public SSAS001(SkillNode skillNode) : base(skillNode)
    {
    }

    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition, direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);
        SetParticleStartRotationFromDeg(leftDeg, rightDeg, downDeg, upDeg);

        dir = direction;
        pos = playerPosition;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //TODO: 감지 모양 및 크기는 추후 수정
            overlapSize = new Vector2(3f, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, 3f);
        }

        Collider2D[] colliders =
            Physics2D.OverlapBoxAll(playerPosition + (direction.normalized * 1f), overlapSize, 0f, monsterLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<Monster>(out Monster monster))
            {
                monster.TakeDamage(skillNode.skillData.SkillDamage);
            }
        }
    }

    public override void ApplyLevelUpStats()
    {
        Debug.Log($"{skillNode.skillData.SkillName} 스킬 스탯 조정");
    }
    
    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }
}