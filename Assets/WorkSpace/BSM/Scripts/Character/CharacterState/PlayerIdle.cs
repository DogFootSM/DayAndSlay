using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    private const string UpDir = "w";
    private const string DownDir = "s";
    private const string RightDir = "d";
    private const string LeftDir = "a";

    private int playIdleHash;
    
    public PlayerIdle(PlayerController playerController) : base(playerController){}

    public override void Enter()
    {
        playIdleHash = playerController.LastKey switch
        {
            "w" => upIdleAnimHash,
            "s" => downIdleAnimHash,
            "a" => leftIdleAnimHash,
            "d" => rightIdleAnimHash    
        };
        
        playerController.CharacterAnimator.Play(playIdleHash); 
    }

    public override void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift) && playerController.moveDir != Vector2.zero)
        {
            playerController.ChangeState(CharacterStateType.WALK);
        }
        
    }
    
}
