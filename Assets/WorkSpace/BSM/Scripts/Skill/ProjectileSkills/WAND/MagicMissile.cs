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
    private int detectedCount;
    private float damage;
    
    private void Awake()
    {
        monsterLayer = LayerMask.GetMask("Monster");
        
        particleInteraction = GetComponent<ParticleInteraction>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer & monsterLayer) != 0) && detectedCount > 0)
        {
            detectedCount--;
            Hit(collider.GetComponent<IEffectReceiver>());
            particleInteraction.IsProjectileStopped = true;
            particleInteraction.PlayHitEffect(transform.position, transform.right);
        }
    }
    
    public void SetData(int detectedCount, int hitCount, float damage)
    {
        this.hitCount = hitCount;
        this.damage = damage;
        this.detectedCount = detectedCount;
    }
   
    
    private void Hit(IEffectReceiver receiver)
    {
        for (int i = 0; i < hitCount; i++)
        {
            receiver.TakeDamage(damage);
        }
    } 
}