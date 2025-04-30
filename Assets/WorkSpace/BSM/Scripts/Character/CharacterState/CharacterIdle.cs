using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdle : CharacterState
{
    
    public CharacterIdle(CharacterController characterController) : base(characterController){}

    public override void Enter()
    {
        Debug.Log("Idle 상태 진입");
    }

    public override void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && characterController.moveDir != Vector2.zero)
        {
            characterController.ChangeState(CharacterStateType.WALK);
        }
        
    }
    
}
