using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAS003 : MeleeSkill
{
    public SSAS003(SkillNode skillNode) : base(skillNode)
    {
    }

    private Vector2 dir;
    private Vector2 pos; 
    
    public override void UseSkill(Vector2 direction, Vector2 playerPosition)
    {
        MeleeEffect(playerPosition, direction, skillNode.skillData.SkillId, skillNode.skillData.SkillEffectPrefab);

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
        
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(playerPosition + (direction.normalized) , overlapSize, 0, monsterLayer);
        
        KnockBackEffect(playerPosition, direction, overlaps[0].GetComponent<IEffectReceiver>());
    }

    public override void ApplyPassiveEffects()
    {
         
    }

    public override void Gizmos()
    {
        UnityEngine.Gizmos.color = Color.blue;

        UnityEngine.Gizmos.DrawWireCube(pos + (dir.normalized * 1f), overlapSize);
    }

    public override float GetSkillDamage()
    {
        return 0;
    }
}
