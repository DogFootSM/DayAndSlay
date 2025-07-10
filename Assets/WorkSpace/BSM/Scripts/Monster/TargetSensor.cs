using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private AstarPath astar;
    [SerializeField] private CircleCollider2D findCollider; 
  
    private LayerMask targetLayer;
    protected float findRange = 5f;

    //아래로 이재호가 추가한 변수
    GameObject player;
    float interval = 0.2f;
    float nextCheckTime = 0f;
    
    public Grid grid;
    Vector3Int lastPlayerCell;


    private void Awake()
    {
        findCollider.radius = findRange;
        targetLayer = LayerMask.GetMask("Player");

        //이재호가 쓴 코드
        player = GameObject.FindWithTag("Player");

        //임시확인용 코드
        if (transform.GetComponentInParent<NPC>())
            player = GameObject.Find("OutsideDoor");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        lastPlayerCell = grid.WorldToCell(player.transform.position);

        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            astar.DetectTarget(transform.position, other.gameObject.transform.position);
        }
    }

    /// <summary>
    /// 이재호가 작성한 코드
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
