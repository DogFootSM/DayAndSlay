using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamEffect : Effect
{
    protected override void Tick()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("TakeDamage > Player");
        }
    }
}
