using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private int playIdleHash;
    private StringBuilder sb;
    
    
    public PlayerIdle(PlayerController playerController) : base(playerController){}

    public override void Enter()
    {
        sb = new StringBuilder(playerController.LastKey);

        //키 동시 여러개 입력 방지
        if (sb.Length > 1)
        {
            sb.Remove(1, sb.Length - 1);   
            playerController.LastKey = sb.ToString();
        } 
        
        //키 입력 시 CapsLock 활성화 예외 처리
        playIdleHash = playerController.LastKey.ToLower() switch
        {
            UpDir => upIdleHash,
            DownDir => downIdleHash,
            LeftDir => leftIdleHash,
            RightDir => rightIdleHash,
            _ => playIdleHash
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
        
    } 
    
}
