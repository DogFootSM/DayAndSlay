using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected PlayerController playerController;
    
    protected int upIdleAnimHash = Animator.StringToHash("CharacterUpIdleAnim");
    protected int downIdleAnimHash = Animator.StringToHash("CharacterIdleAnim");
    protected int leftIdleAnimHash = Animator.StringToHash("CharacterLeftIdleAnim");
    protected int rightIdleAnimHash = Animator.StringToHash("CharacterRightIdleAnim");
    
    
    protected int walkBlendTreeHash = Animator.StringToHash("CharacterWalkBlend");
    
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
    }
     
}
