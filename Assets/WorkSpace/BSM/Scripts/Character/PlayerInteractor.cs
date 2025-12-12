using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private LayerMask detectionLayerMask; 
    private LayerMask npcLayerMask;
    private RaycastHit2D hit;
    private Vector2 curDir;
    
    private int ignorePlayerLayer;
     
    private void Awake()
    {
        detectionLayerMask = LayerMask.GetMask("SavePoint", "NPC", "Door");
        npcLayerMask = LayerMask.GetMask("NPC");
        ignorePlayerLayer = ~(1 << LayerMask.NameToLayer("Player")); 
    }

    private void Update()
    {
        SetRayDirection();
        
        hit = Physics2D.Raycast(transform.position, curDir, 1.5f, detectionLayerMask & ignorePlayerLayer);

        if (hit.collider != null)
        { 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hit.collider.TryGetComponent<GameSaveHandler>(out GameSaveHandler saveHandler))
                {
                    saveHandler.OpenSaveAlert();
                }
                
                else if ((1 << hit.collider.gameObject.layer & npcLayerMask) != 0)
                {
                    NpcTalk npcTalk = hit.collider.gameObject.GetComponentInChildren<NpcTalk>();
                    npcTalk.OnTalkPrintEvent?.Invoke();
                }
                else if (hit.collider.TryGetComponent<InteractableObj>(out InteractableObj interactableObj))
                {
                    interactableObj.Interaction();
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
