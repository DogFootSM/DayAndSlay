using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPath : MonoBehaviour
{
    [SerializeField] private Grid mapGrid;
 
    
    public Vector2Int startPos;
    public Vector2Int targetPos;

    private void Awake()
    {
        startPos = (Vector2Int)mapGrid.WorldToCell(transform.position);
    }

    public void DetectTarget(Vector2 startPos, Vector2 targetPos)
    {
        this.startPos = (Vector2Int)mapGrid.WorldToCell(startPos);
        this.targetPos = (Vector2Int)mapGrid.WorldToCell(targetPos);
        Debug.Log("플레이어 감지, 플레이어 추적 시작");
        Trace();
    }

    private void Trace()
    {
        
    }
    
}