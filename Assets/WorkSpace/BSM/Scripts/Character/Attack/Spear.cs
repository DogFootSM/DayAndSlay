using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spear : IAttackHandler
{
    private Vector2 overlapSize;
    private LayerMask monsterLayer = LayerMask.GetMask("Monster");
    private MonsterAI targetMonster;
    
    private float distance;
    private Vector3 pos;
    private Vector3 dir;
    
    public Spear()
    {
    }

    /// <summary>
    /// 캐릭터 기본 원거리 공격
    /// </summary>
    /// <param name="direction">공격 방향</param>
    /// <param name="position">캐릭터 위치</param>
    /// <param name="itemData"></param>
    /// <param name="playerModel"></param>
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
#if UNITY_EDITOR
        //기즈모 테스트용 pos,dir 코드
        pos = position;
        dir = direction;
        distance = itemData.Range;
#endif
            
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            overlapSize = new Vector2(itemData.Range, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, itemData.Range);
        }

        Collider2D[] monsterCol =
            Physics2D.OverlapBoxAll(position + (direction.normalized * itemData.Range / 2), overlapSize, 0, monsterLayer);
        
        if (monsterCol.Length < 1) return;

        Sort.SortMonstersByNearest(monsterCol, position);
        
        targetMonster = monsterCol[0].GetComponent<MonsterAI>();
        targetMonster.TakeDamage(playerModel.FinalPhysicalDamage);
    }
  
    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * distance / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gizmoPos, overlapSize);
    }
}