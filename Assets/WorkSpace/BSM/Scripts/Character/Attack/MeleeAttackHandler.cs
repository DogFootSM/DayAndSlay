using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHandler : IAttackHandler
{

    private Vector3 pos;
    private Vector3 dir;
    
    
    public MeleeAttackHandler()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기? 아니면 Weapon 클래스에서??
        
        Debug.Log("근접 공격 생성");
    }
    
    
    public void Attack(Vector2 direction, Vector2 position)
    {
        pos = position;
        dir = direction;
        
        Debug.Log("나는 근접 공격!");
    }

    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 1f);
        Vector3 offset = new Vector3(dir.x, dir.y, 0);
        Vector3 size = Vector3.zero;
        
        Gizmos.color = Color.yellow;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            size = new Vector3(2f, 1f, 1f);
        }
        else
        {
            size = new Vector3(1f, 2f, 1f);
        }
         
        
        Gizmos.DrawWireCube(gizmoPos + offset, size);
    }
    
}
