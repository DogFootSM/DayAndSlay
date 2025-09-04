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
        if (!Input.GetKey(KeyCode.LeftShift) && playerController.MoveDir != Vector2.zero)
        {
            playerController.ChangeState(CharacterStateType.WALK);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //TODO: 장착 무기 없을 때 처리 변경하기
            if (playerController.CurrentWeaponType == CharacterWeaponType.LONG_SWORD) return;
            playerController.ChangeState(CharacterStateType.ATTACK);
        }

        CheckSkillKeyInput();
    } 
    

    
}