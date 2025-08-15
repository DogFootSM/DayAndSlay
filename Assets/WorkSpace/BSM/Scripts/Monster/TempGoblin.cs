using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGoblin : Monster
{
    private void OnTriggerStay2D(Collider2D other)
    {
        player.TakeDamage(this, 3f);
    }

    protected override void KnockBack(Vector2 playerPos, Vector2 playerDir)
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
}
