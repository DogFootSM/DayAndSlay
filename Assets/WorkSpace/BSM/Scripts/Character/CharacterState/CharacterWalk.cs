using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalk : CharacterState
{
    public CharacterWalk(CharacterController characterController) : base(characterController){}
    
    public override void Enter()
    {
        characterController.CharacterAnimator.Play(walkBlendTreeHash);
    }

    public override void Update()
    {
        characterController.CharacterAnimator.SetFloat(walkPosXHash, characterController.moveDir.x);
        characterController.CharacterAnimator.SetFloat(walkPosYHash, characterController.moveDir.y);
      
        if (characterController.moveDir == Vector2.zero)
        { 
            characterController.ChangeState(CharacterStateType.IDLE);
        }
    }
    
    public override void FixedUpdate()
    {
        Vector3 dir = new Vector3(characterController.moveDir.x, characterController.moveDir.y, 0);

        characterController.CharacterRb.MovePosition(characterController.transform.position +
                                                     dir * characterController.CharacterModel.MoveSpeed *
                                                     Time.fixedDeltaTime);
    }

    public override void Exit()
    {
        characterController.IsDownWalk = false;
        characterController.IsUpWalk = false;
    }
 
    
}
