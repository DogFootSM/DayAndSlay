using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritRush : MonoBehaviour
{
    private Coroutine rushCoroutine;
    
    public void Rush()
    {
        rushCoroutine = StartCoroutine(RushCoroutine());
    }

    private IEnumerator RushCoroutine()
    {
        while(true)
        {
            transform.position += Vector3.up;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EndRush()
    {
        StopCoroutine(rushCoroutine);
        rushCoroutine = null;
        transform.position = Vector3.zero;
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            //기절
            //데미지 처리
        }
    }
}
