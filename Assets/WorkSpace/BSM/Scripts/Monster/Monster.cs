using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [NonSerialized] public float hp = 100;
    protected float moveSpeed = 3f;
    protected float knockBackPower = 3f;
    
    protected Rigidbody2D rb;
    protected Coroutine knockBackCo;
    protected Coroutine dotDurationCo;
    protected Coroutine dotDamageCo;
    protected Coroutine stunCo;
    protected Coroutine slowCo;
    
    protected Vector2 knockBackDir;
     
    public PlayerController player;
    private Vector3 target;

    protected bool isStunned;
    protected bool isDot;
    
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

    private void Update()
    {
        if (isStunned) return;
        
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
    }

    //-----------------------------------------------
    public void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} 남은 hp :{hp}");
    } 
}
