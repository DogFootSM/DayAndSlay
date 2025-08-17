using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IEffectReceiver
{
    [NonSerialized] public float hp = 100;

    private float defense = 15f;
    
    protected float moveSpeed = 3f;
    protected float knockBackPower = 3f;
    
    protected Rigidbody2D rb;
    protected Coroutine knockBackCo;
    protected Coroutine dotDurationCo;
    protected Coroutine dotDamageCo;
    protected Coroutine stunCo;
    protected Coroutine slowCo;
    protected Coroutine defenseDeBuffCo;
    
    protected Vector2 knockBackDir;
     
    public PlayerController player;
    private Vector3 target;

    protected bool isStunned;
    protected bool isDot;
    protected bool isDefenseDeBuffed;

    private float defenseDeBuffRatio;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TestTrace());
    }

    private IEnumerator TestTrace()
    {
        while (true)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");

            if (go != null)
            {
                player = go.GetComponent<PlayerController>();
            }
             
            if (player != null) break;
            
            yield return WaitCache.GetWait(0.5f);
        }

        while (true)
        {
            target = player.transform.position;
            yield return WaitCache.GetWait(0.5f);
            
        }
    }

    protected void Update()
    {
        if (isStunned) return;
        
        //transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
    }

    //-----------------------------------------------
    public void TakeDamage(float damage)
    {
        float calcDefense = defense;
        
        //TODO: 임시 디버프 방어력 계산
        if (isDefenseDeBuffed)
        {
            calcDefense -= CalculateDefenseDeBuff();
        }
        
        //Debug.Log($"방어력 :{calcDefense}");
        
        //TODO: 몬스터 피해 공식 수정 필요
        hp -= damage;
        Debug.Log($"{gameObject.name} 남은 hp :{hp}");
    }

    /// <summary>
    /// 디버프에 따른 방어력 계산
    /// </summary>
    /// <returns></returns>
    private float CalculateDefenseDeBuff()
    {
        return defense * defenseDeBuffRatio;
    }
    
    public void ReceiveKnockBack(Vector2 playerPos, Vector2 playerDir)
    {
        KnockBack(playerPos, playerDir);
    }

    protected virtual void KnockBack(Vector2 playerPos, Vector2 playerDir)
    {
        //몬스터에 따른 특성 구현
    }
    
    public void ReceiveDot(float duration, float tick, float damage)
    {
        Dot(duration, tick, damage);
    }

    protected virtual void Dot(float duration, float tick, float damage)
    {
        //몬스터에 따른 특성 구현
    }
    
    public void ReceiveStun(float duration)
    {
        Stun(duration);
    }

    protected virtual void Stun(float duration)
    {
        //몬스터에 따른 특성 구현
    }
    
    public void ReceiveSlow(float duration, float ratio)
    {
        Slow(duration, ratio);
    }
    
    protected virtual void Slow(float duration, float ratio)
    {
        //몬스터에 따른 특성 구현
    }
    
    public void ReceiveDefenseDeBuff(float duration, float deBuffPercent)
    {
        defenseDeBuffRatio = deBuffPercent;
        
        DefenseDeBuff(duration);
    }

    protected virtual void DefenseDeBuff(float duration)
    {
        if (defenseDeBuffCo != null)
        {
            StopCoroutine(defenseDeBuffCo);
            defenseDeBuffCo = null;
        }

        defenseDeBuffCo = StartCoroutine(DefenseDeBuffRoutine(duration));
    }

    protected virtual IEnumerator DefenseDeBuffRoutine(float duration)
    {
        float elaspedTime = 0f;
    
        isDefenseDeBuffed = true;
        
        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        isDefenseDeBuffed = false;
    }
    
    /// <summary>
    /// 넉백 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator KnockBackRoutine()
    {
        rb.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);

        yield return WaitCache.GetWait(0.3f);

        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// 도트 지속 시간 코루틴
    /// </summary>
    /// <param name="duration"></param>
    protected virtual IEnumerator DotDurationRoutine(float duration)
    {
        float elapsedTime = 0f;

        isDot = true;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isDot = false;
    }
    
    /// <summary>
    /// 도트 데미지 틱 계산 코루틴
    /// </summary>
    /// <param name="tick"></param>
    /// <param name="perSecondDamage"></param>
    /// <returns></returns>
    protected virtual IEnumerator DotDamageRoutine(float tick, float perSecondDamage)
    {
        while (isDot)
        {
            yield return WaitCache.GetWait(tick);
            TakeDamage(perSecondDamage);
        }
    }
    
    /// <summary>
    /// 몬스터 스턴 효과 코루틴
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    protected virtual IEnumerator StunRoutine(float duration)
    {
        float elapsedTime = 0f;

        isStunned = true;
        
        rb.velocity = Vector2.zero;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isStunned = false;
    }
    
    protected virtual IEnumerator SlowRoutine(float duration, float ratio)
    {
        float elapsedTime = 0f;

        float originMoveSpeed = moveSpeed;

        moveSpeed -= 2f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        moveSpeed = originMoveSpeed;
    }
}
