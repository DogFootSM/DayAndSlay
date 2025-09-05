using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D arrowRb;

    private ArrowPool arrowPool => ArrowPool.Instance;
    private Coroutine returnCo;
    
    private Vector2 startPos = new Vector2();
    private LayerMask monsterLayer; 
    
    private float damage;
    private float range;
    private float arrowSpeed = 15f;
    
    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & monsterLayer) != 0)
        {
            Monster monster = other.gameObject.GetComponent<Monster>();
            monster.TakeDamage(damage);
            arrowPool.ReturnPoolArrow(this.gameObject); 
        }
    }

    private void OnDisable()
    {
        if (returnCo != null)
        {
            StopCoroutine(returnCo);
            returnCo = null;
        }
    }

    /// <summary>
    /// 화살이 날라갈 방향 및 회전 설정
    /// </summary>
    /// <param name="pos">화살이 생성될 위치</param>
    /// <param name="dir">발사 방향에 따른 화살 회전</param>
    public void SetLaunchTransform(Vector2 pos, Vector2 dir, float weaponRange)
    {
        transform.position = pos;
        startPos = pos;
        range = weaponRange;
        
        //TODO: 화살은 모두 속도 동일로, 적정 속도 찾아서 상수값으로 박기
        arrowRb.AddForce(dir * arrowSpeed, ForceMode2D.Impulse);

        returnCo = StartCoroutine(ArrowReturnRoutine());
    }
 
    /// <summary>
    /// 화살이 공격 가능 최대 사거리 도달 시 풀에 반환하는 코루틴 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ArrowReturnRoutine()
    {
        while (Vector2.Distance(transform.position, startPos) < range)
        {
            transform.right = -arrowRb.velocity;
            yield return null;
        } 
        arrowPool.ReturnPoolArrow(this.gameObject);
    }
    
    /// <summary>
    /// 화살의 데미지 설정
    /// </summary>
    /// <param name="damage">캐릭터의 레벨 및 스탯에 따른 데미지를 화살에 적용</param>
    public void SetArrowDamage(float damage)
    {
        this.damage = damage;
    }
}