using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGoblin : Monster
{
    protected override void KnockBack(Vector2 playerPos)
    {
        Vector2 distance = playerPos - new Vector2(transform.position.x, transform.position.y);

        if (knockBackCo != null)
        {
            StopCoroutine(knockBackCo);
            knockBackCo = null;
        }

        knockBackCo = StartCoroutine(KnockBackRoutine());
    }

    protected override void Dot(float duration, float tick, float damage)
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


    protected override void Stun(float duration)
    {
        if (stunCo != null)
        {
            StopCoroutine(stunCo);
            stunCo = null;
        }

        stunCo = StartCoroutine(StunRoutine(duration));
    }
   
    protected override void Slow(float duration, float ratio)
    {
        if (slowCo != null)
        {
            StopCoroutine(slowCo);
            slowCo = null;
        }

        slowCo = StartCoroutine(SlowRoutine(duration, ratio)); 
    }

    protected override void AttackDeBuff(float duration, float deBuffPer)
    {
        if (attackDeBuffCo == null)
        {
            attackDeBuffCo = StartCoroutine(AttackDeBuffRoutine(duration, deBuffPer));
        } 
    }
    
}
