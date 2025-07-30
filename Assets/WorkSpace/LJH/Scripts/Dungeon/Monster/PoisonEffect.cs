using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : Effect
{
    private float damageCooldown = 1f; // 초당 1번 데미지
    private float timer = 0f;

    protected override void Tick()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && timer <= 0f)
        {
            Debug.Log("피해입음");
            timer = damageCooldown;
        }
    }
}
