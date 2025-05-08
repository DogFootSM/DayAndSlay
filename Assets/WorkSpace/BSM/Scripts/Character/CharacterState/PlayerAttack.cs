using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerAttack : PlayerState
{
    private int attackHash;
    private StringBuilder sb;
    
    public PlayerAttack(PlayerController playerController) : base(playerController){}

    public override void Enter()
    { 
        sb = new StringBuilder(playerController.LastKey);

        //키 동시 여러개 입력 방지
        if (sb.Length > 1)
        {
            sb.Remove(1, sb.Length - 1);   
            playerController.LastKey = sb.ToString();
        } 
        
        attackHash = playerController.LastKey.ToLower() switch
        {
            UpDir => upAttackAnimHash,
            DownDir => downAttackAnimHash,
            LeftDir => leftAttackAnimHash,
            RightDir => rightAttackAnimHash
        };
        
        playerController.CharacterAnimator.Play(attackHash);
    }
    
}
