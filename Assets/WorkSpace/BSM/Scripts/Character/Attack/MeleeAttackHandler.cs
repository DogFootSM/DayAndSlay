using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackHandler : IAttackHandler
{

    private Vector3 pos;
    private Vector3 dir;
    private Vector2 overlapSize;

    private LayerMask monsterLayer;
    
    public MeleeAttackHandler()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기? 아니면 Weapon 클래스에서??
        monsterLayer = LayerMask.GetMask("Monster"); 
    }
    
    
    public void NormalAttack(Vector2 direction, Vector2 position)
    {
        //테스트용 pos,dir 코드
        pos = position;
        dir = direction;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //TODO: 추후 공격 사거리 만큼 사이즈 조정 필요
            overlapSize = new Vector2(2f, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, 2f);
        }

        Collider2D col = Physics2D.OverlapBox(position + (direction.normalized * 1f), overlapSize, 0f,monsterLayer);

        if (col == null) return;

        Monster monster = col.GetComponent<Monster>();

        if (monster != null)
        {
            //TODO: 몬스터 데미지는 캐릭터의 기본 공격력 + 무기 데미지 계산해서 데미지를 주면 될듯?
            monster.TakeDamage(5);
        } 
    }

    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 1f); 
  
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(gizmoPos, overlapSize);
    }
    
}
