using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private AstarPath astar;
    [SerializeField] private CircleCollider2D findCollider; 
  
    private LayerMask targetLayer;
    private float findRange = 5f;

    GameObject player;
    float interval = 0.2f;
    float nextCheckTime = 0f;
    [SerializeField] private Grid grid;
    Vector3Int lastPlayerCell;


    private void Awake()
    {
        findCollider.radius = findRange;
        targetLayer = LayerMask.GetMask("Player");

        //이재호가 추가한 코드
        player = GameObject.FindWithTag("Player");
        lastPlayerCell = grid.WorldToCell(player.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {  
            astar.DetectTarget(transform.position, other.gameObject.transform.position);
        }
    }

    /// <summary>
    /// 이재호가 추가한 코드
    /// 경로 재탐색 테스트를 위해 추가함
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3Int currentCell = grid.WorldToCell(player.transform.position);

        if (Time.time >= nextCheckTime)
        {
            if (currentCell != lastPlayerCell)
            {
                astar.DetectTarget(transform.position, player.transform.position);

                lastPlayerCell = currentCell;
            }

            nextCheckTime = Time.time + interval;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
    }
}
