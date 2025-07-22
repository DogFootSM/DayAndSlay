using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    public PlayerWalk(PlayerController playerController) : base(playerController){}
    
    public override void Enter()
    {
        if (playerController.PlayerModel.IsMovementBlocked)
        {
            playerController.ChangeState(CharacterStateType.IDLE);
            return;
        }
        
        playerController.BodyAnimator.Play(walkBlendTreeHash);
        playerController.WeaponAnimator.Play(walkBlendTreeHash); 
    }

    public override void Update()
    { 
        playerController.BodyAnimator.SetFloat(walkPosXHash, playerController.MoveDir.x);
        playerController.BodyAnimator.SetFloat(walkPosYHash, playerController.MoveDir.y); 
        
        playerController.WeaponAnimator.SetFloat(walkPosXHash, playerController.MoveDir.x);
        playerController.WeaponAnimator.SetFloat(walkPosYHash, playerController.MoveDir.y); 
        
        
        if (playerController.MoveDir == Vector2.zero)
        { 
            playerController.ChangeState(CharacterStateType.IDLE);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.ChangeState(CharacterStateType.ATTACK);
        }
        
    }
    
    public override void FixedUpdate()
    {
        Vector3 dir = new Vector3(playerController.MoveDir.x, playerController.MoveDir.y, 0);

        playerController.CharacterRb.MovePosition(playerController.transform.position +
                                                     dir * moveSpeed *
                                                     Time.fixedDeltaTime);
    }

    public override void Exit()
    {

    }
  
}
