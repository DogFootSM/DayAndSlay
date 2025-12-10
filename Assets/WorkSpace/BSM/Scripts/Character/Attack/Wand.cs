using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : IAttackHandler
{
    private Vector2 overlapSize;
    private LayerMask monsterLayer= LayerMask.GetMask("Monster");
    private MonsterAI targetMonster;
    
    private Vector3 pos;
    private Vector3 dir;
    private float distance = 9999f;
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        Debug.Log("완드 일반 공격");
        
        pos = position;
        dir = direction;
        distance = itemData.Range;
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            overlapSize = new Vector2(itemData.Range, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, itemData.Range);
        }
        
        Collider2D[] monsterColliders =
            Physics2D.OverlapBoxAll(position + (direction.normalized * itemData.Range / 2), overlapSize, 0f, monsterLayer);
        
        if (monsterColliders.Length < 1) return;
        
        Sort.SortMonstersByNearest(monsterColliders, position);
        
        targetMonster = monsterColliders[0].GetComponent<MonsterAI>();
        targetMonster.TakeDamage(playerModel.FinalPhysicalDamage);
    }
    
    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * distance / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gizmoPos, overlapSize);
    }
}
