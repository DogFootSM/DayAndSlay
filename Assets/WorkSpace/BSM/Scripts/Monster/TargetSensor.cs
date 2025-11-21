using AYellowpaper.SerializedCollections.Editor.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] protected AstarPath astar;
    [SerializeField] protected CircleCollider2D findCollider;

    protected LayerMask targetLayer;
    protected float findRange = 0f;

    //아래로 이재호가 추가한 변수
    protected GameObject player;
    protected float interval = 0.5f;
    protected float nextCheckTime = 0f;

    private Coroutine detectCoroutine;

    public Grid grid;
    protected Vector3Int lastPlayerCell;

    /// <summary>
    /// findCollider의 radius값은 고정이 아닌 몬스터의 ChaseRange에 따라 변경되도록 설정
    /// </summary>
    private void Awake()
    {
        findCollider.radius = GetComponentInParent<MonsterAI>().GetChaseRange() / 6;
        targetLayer = LayerMask.GetMask("Player");
    }

    private void Start()
    {
        
        astar.SetGridAndTilemap(grid);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEnterMethod(other);
        player = other.gameObject;
    }

    protected virtual void TriggerEnterMethod(Collider2D other)
    {
        lastPlayerCell = grid.WorldToCell(other.transform.position);

        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            astar.DetectTarget(transform.position, other.gameObject.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (!transform.parent.gameObject.activeSelf) return;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponentInParent<MonsterAI>().GetChaseRange());
    }

    public void SetGrid(Grid grid) => this.grid = grid;
}
