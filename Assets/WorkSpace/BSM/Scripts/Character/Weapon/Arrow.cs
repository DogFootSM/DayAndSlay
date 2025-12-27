using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D arrowRb;

    private const float ARROW_SPEED = 12f;
    
    private ArrowPool arrowPool => ArrowPool.Instance;
    private Coroutine arrowPoolReturnCo;
    public MonsterAI monsterAI;
    
    private Vector2 startPos = new Vector2();
    private LayerMask monsterLayer; 
    
    private float damage;
    private float range;
     
    //슬로우 스킬 적용값
    private float slowRatio;
    private float slowDuration;
    private bool isSlowSkill;
    
    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //화살이 몬스터에 닿았을 경우
        if ((1 << other.gameObject.layer & monsterLayer) != 0 && monsterAI == null)
        {
            monsterAI = other.gameObject.GetComponent<MonsterAI>();;
            monsterAI.TakeDamage(damage);
             
            //현재 화살이 슬로우 스킬 화살이며 몬스터가 슬로우 적용중이지 않은 상태
            if (isSlowSkill && !monsterAI.IsSlow)
            {
                IEffectReceiver receiver = other.gameObject.GetComponent<IEffectReceiver>();
                receiver.ReceiveSlow(slowDuration, slowRatio); 
            }
            arrowPool.ReturnPoolArrow(this.gameObject); 
        } 
    }

    private void OnEnable()
    {
        monsterAI = null; 
    }

    private void OnDisable()
    {
        isSlowSkill = false; 
        slowDuration = 0f;
        slowRatio = 0f; 
        
        if (arrowPoolReturnCo != null)
        {
            StopCoroutine(arrowPoolReturnCo);
            arrowPoolReturnCo = null;
        }
    }

    /// <summary>
    /// 화살이 날라갈 방향 및 회전 설정
    /// </summary>
    /// <param name="pos">화살이 생성될 위치</param>
    /// <param name="dir">발사 방향에 따른 화살 회전</param>
    /// <param name="weaponRange">화살이 날라갈 최대 사거리</param>
    public void SetLaunchTransform(Vector2 pos, Vector2 dir, float weaponRange)
    {
        transform.position = pos;
        startPos = pos;
        range = weaponRange;
        arrowRb.AddForce(dir * ARROW_SPEED, ForceMode2D.Impulse);

        arrowPoolReturnCo = StartCoroutine(ArrowReturnRoutine());
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

    /// <summary>
    /// 화살 오브젝트에 슬로우 효과 적용
    /// </summary>
    /// <param name="isSlowSkill">슬로우 스킬 적용 여부</param>
    /// <param name="slowDuration">슬로우 지속 시간</param>
    /// <param name="slowRatio">슬로우 비율</param>
    public void SetSlowSkill(bool isSlowSkill, float slowDuration, float slowRatio)
    {
        this.isSlowSkill = isSlowSkill;
        this.slowDuration = slowDuration;
        this.slowRatio = slowRatio;
    }
    
}