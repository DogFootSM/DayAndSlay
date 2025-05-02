using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    
    public PlayerIdle(PlayerController playerController) : base(playerController){}

    public override void Enter()
    {
        for (int i = 0; i < playerController.PlayerSprites.Count; i++)
        {
            playerController.PlayerSprites[i].flipX = false;
        }
         
        playerController.CharacterAnimator.Play(idleAnimHash);
    }

    public override void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && playerController.moveDir != Vector2.zero)
        {
            playerController.ChangeState(CharacterStateType.WALK);
        }
        
    }
    
}
