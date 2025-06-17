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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            skillKey = KeyCode.Q;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            skillKey = KeyCode.W;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillKey = KeyCode.E;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            skillKey = KeyCode.R;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            skillKey = KeyCode.A;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            skillKey = KeyCode.S;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            skillKey = KeyCode.D;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            skillKey = KeyCode.F;
            playerController.ChangeState(CharacterStateType.SKILL);
        }
    }
}