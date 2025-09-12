using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MagicMissile : MonoBehaviour
{
    private LayerMask monsterLayer;

    private int hitCount;
    private float damage;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster") || collider.CompareTag("Boss") && collider is IEffectReceiver)
        {
            Debug.Log("매직미사일 맞음");
            Hit(collider.GetComponent<IEffectReceiver>(), hitCount, damage);
            IncreaseMagicDamage(collider);
        }
    }
    
    public void SetData(int hitCount, float damage)
    {
        this.hitCount = hitCount;
        this.damage = damage;
    }
   
    
    protected void Hit(IEffectReceiver receiver, int hitCount, float damage)
    {
        for (int i = 0; i < hitCount; i++)
        {
            receiver.TakeDamage(damage);
        }
    }

    //맞는 적 데미지 10% 증가된 상태로 맞는 디버프
    private void IncreaseMagicDamage(Collider2D collider)
    {
        Debug.Log("#초동안 공격력 10% 증가된 상태로 맞음");
    }
    
}