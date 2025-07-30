using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private LayerMask detectionLayerMask; 
    private RaycastHit2D hit;
    private Vector2 curDir;
    
    private int ignorePlayerLayer;
     
    private void Awake()
    {
        detectionLayerMask = LayerMask.GetMask("SavePoint", "NPC");
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player")); 
    }

    private void Update()
    {
        SetRayDirection();
        
        hit = Physics2D.Raycast(transform.position, curDir, 1.5f, detectionLayerMask & ignorePlayerLayer);

        if (hit.collider != null)
        { 
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.TryGetComponent<GameSaveHandler>(out GameSaveHandler saveHandler))
                {
                    saveHandler.OpenSaveAlert();
                }
                else if (hit.collider.transform.GetChild(0).TryGetComponent<NpcTalk>(out NpcTalk npcTalk))
                {
                    npcTalk.OnTalkPrintEvent?.Invoke();
                }
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
