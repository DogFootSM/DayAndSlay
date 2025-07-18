using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGoblin : Monster, IEffectReceiver
{
    
    /// <summary>
    /// 넉백 리시버
    /// 몬스터에 따른 넉백 효과 구현
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerDir"></param>
    public void ReceiveKnockBack(Vector2 playerPos, Vector2 playerDir)
    {
        Vector2 distance = playerPos - new Vector2(transform.position.x, transform.position.y);
        
        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (playerDir.x > 0)
            {
                //오른쪽 방향으로
                knockBackDir = Vector2.right;
            }
            else
            {
                //왼쪽 방향으로
                knockBackDir = Vector2.left;
            }
        }
        else
        {
            if (playerDir.y > 0)
            {
                //윗 방향으로
                knockBackDir = Vector2.up;
            }
            else
            {
                //아랫 방향으로
                knockBackDir = Vector2.down;
            }
        }

        if (knockBackCo != null)
        {
            StopCoroutine(knockBackCo);
            knockBackCo = null;
        }

        knockBackCo = StartCoroutine(KnockBackRoutine());
    }

    /// <summary>
    /// 넉백 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator KnockBackRoutine()
    {
        rb.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);

        yield return WaitCache.GetWait(0.3f);

        rb.velocity = Vector2.zero;
    }
    
    /// <summary>
    /// 도트 데미지 리시버
    /// 몬스터에 따른 도트 데미지 구현
    /// </summary>
    /// <param name="duration">도트 데미지의 지속 시간</param>
    /// <param name="tick">몬스터에게 데미즈를 가할 시간 간격</param>
    /// <param name="damage">도트 데미지 수치</param>
    public void ReceiveDot(float duration, float tick, float damage)
    {
        if (dotDurationCo != null)
        {
            StopCoroutine(dotDurationCo);
            dotDurationCo = null;
        }

        if (dotDamageCo != null)
        {
            StopCoroutine(dotDamageCo);
            dotDamageCo = null;
        }
        
        dotDurationCo = StartCoroutine(DotDurationRoutine(duration));
        dotDamageCo = StartCoroutine(DotDamageRoutine(tick, damage));
    }

    /// <summary>
    /// 도트 지속 시간 코루틴
    /// </summary>
    /// <param name="duration"></param>
    private IEnumerator DotDurationRoutine(float duration)
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
    private IEnumerator DotDamageRoutine(float tick, float perSecondDamage)
    {
        while (isDot)
        {
            yield return WaitCache.GetWait(tick);
            TakeDamage(perSecondDamage);
        }
    }

    /// <summary>
    /// 스턴 효과 리시버
    /// 몬스터에 따른 스턴 효과 구현
    /// </summary>
    /// <param name="duration">스턴 지속 시간</param>
    public void ReceiveStun(float duration)
    {
        if (stunCo != null)
        {
            StopCoroutine(stunCo);
            stunCo = null;
        }

        stunCo = StartCoroutine(StunRoutine(duration));
    }

    /// <summary>
    /// 몬스터 스턴 효과 코루틴
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator StunRoutine(float duration)
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
}
