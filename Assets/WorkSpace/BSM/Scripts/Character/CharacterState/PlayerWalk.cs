using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerController playerController) : base(playerController){}
    
    public override void Enter()
    {
        playerController.CharacterAnimator.Play(walkBlendTreeHash);
    }

    public override void Update()
    {
        playerController.CharacterAnimator.SetFloat(walkPosXHash, playerController.moveDir.x);
        playerController.CharacterAnimator.SetFloat(walkPosYHash, playerController.moveDir.y);
      
        if (playerController.moveDir == Vector2.zero)
        { 
            playerController.ChangeState(CharacterStateType.IDLE);
        }
    }
    
    public override void FixedUpdate()
    {
        Vector3 dir = new Vector3(playerController.moveDir.x, playerController.moveDir.y, 0);

        playerController.CharacterRb.MovePosition(playerController.transform.position +
                                                     dir * playerController.PlayerModel.MoveSpeed *
                                                     Time.fixedDeltaTime);
    }

    public override void Exit()
    {
        playerController.IsDownWalk = false;
        playerController.IsUpWalk = false;
    }
 
    
}
