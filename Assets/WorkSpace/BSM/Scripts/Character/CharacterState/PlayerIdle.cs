using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private int playIdleHash; 
 
    public PlayerIdle(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        playIdleHash = playerController.LastMoveKey switch
        {
            Direction.Up => upIdleHash,
            Direction.Down => downIdleHash,
            Direction.Right => leftIdleHash,
            Direction.Left => rightIdleHash,
            _ => downIdleHash
        };

        playerController.BodyAnimator.Play(playIdleHash);
        playerController.WeaponAnimator.Play(playIdleHash);
    }

    public override void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && playerController.moveDir != Vector2.zero)
        {
            playerController.ChangeState(CharacterStateType.WALK);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerController.ChangeState(CharacterStateType.ATTACK);
        }

        CheckSkillKeyInput();
    } 
    

    
}