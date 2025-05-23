using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private AstarPath astar;
    [SerializeField] private CircleCollider2D findCollider; 
  
    private LayerMask targetLayer;
    private float findRange = 2f;

    private void Awake()
    {
        findCollider.radius = findRange;
        targetLayer = LayerMask.GetMask("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        { 
            astar.DetectTarget(transform.position, other.gameObject.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
    }
}
