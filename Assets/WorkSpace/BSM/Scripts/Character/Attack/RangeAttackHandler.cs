using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
   
public class RangeAttackHandler : IAttackHandler
{
    
    private Vector3 pos;
    private Vector3 dir;
    private LayerMask monsterLayer;
    private Monster targetMonster;
    private float distance = 9999f;
     
    public RangeAttackHandler()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기?
        monsterLayer = LayerMask.GetMask("Monster"); 
    }
    
    public void NormalAttack(Vector2 direction, Vector2 position)
    {
        pos = position;
        dir = direction;
        Debug.Log("나는 원거리 공격!");

        Collider2D[] monsterCol = Physics2D.OverlapCircleAll(position + (direction.normalized * 3f), 3f, monsterLayer);
 
        if (monsterCol.Length < 1) return;
         
        //타겟이었던 몬스터가 공격 범위 내에서 멀어졌을 경우 새 타겟을 탐색
        if (targetMonster != null && !monsterCol.Contains(targetMonster.gameObject.GetComponent<Collider2D>()))
        {
            distance = 9999f;
            
            for (int i = 0; i < monsterCol.Length; i++)
            {
                float compareDistance = Vector2.Distance(position, monsterCol[i].transform.position);
                
                if (distance > compareDistance)
                {
                    distance = compareDistance;
                    targetMonster = monsterCol[i].gameObject.GetComponent<Monster>();
                } 
            }
        } 
         
        if (targetMonster != null)
        { 
            targetMonster.TakeDamage(5);
        }
        
    }
  
    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 3f);
        Gizmos.color = Color.yellow;
 
        Gizmos.DrawWireSphere(gizmoPos, 3f);
    }
    
}
