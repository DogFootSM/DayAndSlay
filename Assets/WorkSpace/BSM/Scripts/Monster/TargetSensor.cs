using AYellowpaper.SerializedCollections.Editor.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] protected AstarPath astar;
    [SerializeField] protected CircleCollider2D findCollider; 
  
    protected LayerMask targetLayer;
    protected float findRange = 5f;

    //아래로 이재호가 추가한 변수
    protected GameObject player;
    protected float interval = 0.2f;
    protected float nextCheckTime = 0f;
    
    public Grid grid;
    protected Vector3Int lastPlayerCell;


    private void Awake()
    {
        findCollider.radius = findRange;
        targetLayer = LayerMask.GetMask("Player");
        astar.SetGridAndTilemap(grid);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnterMethod(other);
    }

    protected virtual void TriggerEnterMethod(Collider2D other)
    {
        lastPlayerCell = grid.WorldToCell(other.transform.position);

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
        TriggerStayMethod(collision);
    }

    protected virtual void TriggerStayMethod(Collider2D collision)
    {
        Vector3Int currentCell = grid.WorldToCell(collision.transform.position);

        if (Time.time >= nextCheckTime)
        {
            if (currentCell != lastPlayerCell)
            {
                astar.DetectTarget(transform.position, collision.transform.position);

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
