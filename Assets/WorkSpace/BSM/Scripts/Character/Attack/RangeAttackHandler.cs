using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
public class RangeAttackHandler : IAttackHandler
{
    
    private Vector3 pos;
    private Vector3 dir;
 
    public RangeAttackHandler()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기?
        
        Debug.Log("원거리 공격 생성");
    }
    
    public void Attack(Vector2 direction, Vector2 position)
    {
        pos = position;
        dir = direction;
        Debug.Log("나는 원거리 공격!");
    }
  
    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 1f);
        Gizmos.color = Color.yellow;
 
        Gizmos.DrawWireSphere(gizmoPos, 3f);
    }
    
}
