using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [NonSerialized] public float hp = 100;

    private Rigidbody2D rb;
    private Coroutine knockBackCo;
    private Coroutine bleedDurationCo;
    private Coroutine bleedDamageCo;
    
    private bool isBleed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} 남은 hp :{hp}");
    }

    /// <summary>
    /// 넉백 스킬 효과 적용
    /// </summary>
    /// <param name="knockBackDir"></param>
    public void ReceiveKnockBack(Vector2 knockBackDir)
    {
        if (knockBackCo != null)
        {
            StopCoroutine(knockBackCo);
            knockBackCo = null;
        }

        knockBackCo = StartCoroutine(KnockBackRoutine(knockBackDir));
    }

    /// <summary>
    /// 출혈 스킬 효과 적용
    /// </summary>
    /// <param name="duration">출혈 효과 지속 시간</param>
    /// <param name="tick">몇 초당 데미지를 부여할지에 대한 시간</param>
    /// <param name="perSecondDamage">초당 지속 데미지</param>
    public void ReceiveBleedDamage(float duration, float tick, float perSecondDamage)
    {
        if (isBleed) return;
        
        if (bleedDurationCo != null)
        {
            StopCoroutine(bleedDurationCo);
            bleedDurationCo = null;
        }

        if (bleedDamageCo != null)
        {
            StopCoroutine(bleedDamageCo);
            bleedDamageCo = null;
        }
        
        bleedDurationCo = StartCoroutine(BleedRoutine(duration));
        bleedDamageCo = StartCoroutine(BleedDamageRoutine(tick, perSecondDamage));
    }
    
    /// <summary>
    /// 넉백 코루틴
    /// </summary>
    /// <param name="knockBackDir">뒤로 이동할 방향</param>
    /// <returns></returns>
    private IEnumerator KnockBackRoutine(Vector2 knockBackDir)
    {
        rb.AddForce(knockBackDir * 1f, ForceMode2D.Impulse);
        yield return WaitCache.GetWait(0.3f);
        rb.velocity = Vector2.zero;

        if (knockBackCo != null)
        {
            StopCoroutine(knockBackCo);
            knockBackCo = null;
        }
    }

    /// <summary>
    /// 출혈 지속 시간 코루틴
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator BleedRoutine(float duration)
    {
        float elapsedTime = 0;

        isBleed = true;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        isBleed = false;
    }

    /// <summary>
    /// 출혈 데미지 틱 계산 코루틴
    /// </summary>
    /// <param name="tick"></param>
    /// <param name="perSecondDamage"></param>
    /// <returns></returns>
    private IEnumerator BleedDamageRoutine(float tick, float perSecondDamage)
    {
        while (isBleed)
        {
            yield return WaitCache.GetWait(tick);
            TakeDamage(perSecondDamage);
        } 
    }
    
}
