using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveInteractor : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    private LayerMask savePointLayerMask; 
    private RaycastHit2D hit;
     
    private int ignorePlayerLayer;
    private Vector2 curDir;
    
    private void Awake()
    {
        savePointLayerMask = LayerMask.GetMask("SavePoint");
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player")); 
    }

    private void Update()
    {
        SetRayDirection();
        
        Debug.DrawRay(transform.position, curDir * 1.5f, Color.red);

        hit = Physics2D.Raycast(transform.position, curDir, 1.5f, savePointLayerMask & ignorePlayerLayer);

        if (hit.collider != null)
        { 
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameSaveHandler gameSaveHandler = hit.collider.gameObject.GetComponent<GameSaveHandler>();
                gameSaveHandler.OpenSaveAlert();
            } 
        } 
    }

    /// <summary>
    /// 레이의 방향 설정
    /// </summary>
    private void SetRayDirection()
    {
        if(playerController.LastMoveKey == Direction.Down) curDir = Vector2.down;
        if(playerController.LastMoveKey == Direction.Right) curDir = Vector2.left;
        if(playerController.LastMoveKey == Direction.Up) curDir = Vector2.up;
        if(playerController.LastMoveKey == Direction.Left) curDir = Vector2.right;
    }
    
    
}
