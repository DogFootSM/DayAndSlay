using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSword : IAttackHandler
{
    private Vector3 pos;
    private Vector3 dir;
    private Vector2 overlapSize;

    private LayerMask monsterLayer;
    private NewMonsterAI targetMonster;
    private float distance;

    public ShortSword()
    {
        //TODO: 공격범위 및 사거리 DB 불러오기? 아니면 Weapon 클래스에서??
        monsterLayer = LayerMask.GetMask("Monster");
    }

    /// <summary>
    /// 캐릭터 기본 근거리 공격
    /// </summary>
    /// <param name="direction">공격 방향</param>
    /// <param name="position">캐릭터 위치</param>
    /// <param name="itemData"></param>
    /// <param name="playerModel"></param>
    public void NormalAttack(Vector2 direction, Vector2 position, ItemData itemData, PlayerModel playerModel)
    {
        //기즈모 테스트용 pos,dir 코드
        pos = position;
        dir = direction;
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            //TODO: 추후 공격 사거리 만큼 사이즈 조정 필요
            overlapSize = new Vector2(itemData.Range, 1f);
        }
        else
        {
            overlapSize = new Vector2(1f, itemData.Range);
        }

        Collider2D[] monsterColliders =
            Physics2D.OverlapBoxAll(position + (direction.normalized * 1f), overlapSize, 0f, monsterLayer);

        if (monsterColliders.Length < 1) return;

        //감지된 몬스터가 1마리일 경우 바로 타겟 설정
        if (monsterColliders.Length == 1)
        {
            //targetMonster = monsterColliders[0].GetComponent<Monster>();
            targetMonster = monsterColliders[0].GetComponent<NewMonsterAI>();
        }
        else
        {
            TargetMonsterSwitch(position, monsterColliders);
        }


        if (targetMonster != null)
        {
            //TODO: 몬스터 데미지는 캐릭터의 기본 공격력 + 무기 데미지 계산해서 데미지를 주면 될듯?
            targetMonster.TakeDamage(playerModel.FinalPhysicalDamage);
        }
    }

    /// <summary>
    /// 몬스터 타겟 변경
    /// </summary>
    /// <param name="position">현재 캐릭터 위치</param>
    /// <param name="colliders">공격 범위 내 감지된 몬스터</param>
    private void TargetMonsterSwitch(Vector2 position, Collider2D[] colliders)
    {
        distance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            float compareDistance = Vector2.Distance(position, colliders[i].transform.position);

            if (distance > compareDistance)
            {
                distance = compareDistance;
                targetMonster = colliders[i].gameObject.GetComponent<NewMonsterAI>();
            }
        }
    }

    public void DrawGizmos()
    {
        Vector3 gizmoPos = pos + (dir.normalized * 1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gizmoPos, overlapSize);
    }
}