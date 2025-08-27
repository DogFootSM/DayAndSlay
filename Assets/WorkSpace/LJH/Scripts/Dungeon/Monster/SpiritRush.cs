using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritRush : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position += Vector3.up * 5f;
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
