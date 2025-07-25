using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; 

public class Spear : IAttackHandler
{
     private Vector3 pos;
    private Vector3 dir;
    private LayerMask monsterLayer;
    private Monster targetMonster;
    private float distance = 9999f;
     
    public Spear()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기?
        monsterLayer = LayerMask.GetMask("Monster"); 
    }
    
    /// <summary>
    /// 캐릭터 기본 원거리 공격
    /// </summary>
    /// <param name="direction">공격 방향</param>
    /// <param name="position">캐릭터 위치</param>
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData)
    {
        pos = position;
        dir = direction;
        Debug.Log(itemData.Name);
        Debug.Log("창 일반 공격");
        Collider2D[] monsterCol = Physics2D.OverlapCircleAll(position + (direction.normalized * 3f), 3f, monsterLayer);
        
        //TODO: 활 공격 범위도 좌,우, 위, 아래로
        
        if (monsterCol.Length < 1) return;
          
        if (targetMonster == null)
        {
            TargetMonsterSwitch(position, monsterCol);
        }
        else
        {
            //타겟이었던 몬스터가 공격 범위 내에서 멀어졌을 경우 새 타겟을 탐색
            if(!monsterCol.Contains(targetMonster.gameObject.GetComponent<Collider2D>()))
            {
                distance = 9999f;
                TargetMonsterSwitch(position, monsterCol);
            } 
        }
        
        //TODO: 활 공격같은 경우에는 화살 Pool에 타겟을 전달해주고 Pool에서 데미지를 주는 방식
        if (targetMonster != null)
        { 
            targetMonster.
                TakeDamage(5);
        }
        
    }

    /// <summary>
    /// 몬스터 타겟 변경
    /// </summary>
    /// <param name="position">현재 캐릭터 위치</param>
    /// <param name="colliders">공격 범위 내 감지된 몬스터</param>
    private void TargetMonsterSwitch(Vector2 position ,Collider2D[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            float compareDistance = Vector2.Distance(position, colliders[i].transform.position);
                
            if (distance > compareDistance)
            {
                distance = compareDistance;
                targetMonster = colliders[i].gameObject.GetComponent<Monster>();
            } 
        }
    }
    
    
    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 3f);
        Gizmos.color = Color.yellow;
 
        Gizmos.DrawWireSphere(gizmoPos, 3f);
    }
}
