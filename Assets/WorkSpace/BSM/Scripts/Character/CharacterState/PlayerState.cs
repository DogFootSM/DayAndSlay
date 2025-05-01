using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected PlayerController playerController;
    
    protected int idleAnimHash = Animator.StringToHash("CharacterIdleAnim");
    protected int walkBlendTreeHash = Animator.StringToHash("CharacterWalkBlend");
    
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
    }
     
}
