using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MagicMissile : MonoBehaviour
{
    private LayerMask monsterLayer;
    private ParticleInteraction particleInteraction;
    private int hitCount;
    private float damage;
    
    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
        
        particleInteraction = GetComponent<ParticleInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster") || collider.CompareTag("Boss"))
        {
            Hit(collider.GetComponent<IEffectReceiver>(), hitCount, damage);
            particleInteraction.IsProjectileStopped = true;
        }
    }
    
    public void SetData(int hitCount, float damage)
    {
        this.hitCount = hitCount;
        this.damage = damage;
    }
   
    
    private void Hit(IEffectReceiver receiver, int hitCount, float damage)
    {
        for (int i = 0; i < hitCount; i++)
        {
            receiver.TakeDamage(damage);
        }
    } 
}