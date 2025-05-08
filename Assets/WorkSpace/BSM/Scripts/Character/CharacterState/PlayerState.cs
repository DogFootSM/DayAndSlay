using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected const string UpDir = "w";
    protected const string DownDir = "s";
    protected const string RightDir = "d";
    protected const string LeftDir = "a";
    
    protected PlayerController playerController;
    
    protected int upIdleAnimHash = Animator.StringToHash("CharacterUpIdleAnim");
    protected int downIdleAnimHash = Animator.StringToHash("CharacterIdleAnim");
    protected int leftIdleAnimHash = Animator.StringToHash("CharacterLeftIdleAnim");
    protected int rightIdleAnimHash = Animator.StringToHash("CharacterRightIdleAnim");
    protected int downAttackAnimHash = Animator.StringToHash("CharacterDownAttackAnim");
    protected int upAttackAnimHash = Animator.StringToHash("CharacterUpAttackAnim");
    protected int leftAttackAnimHash = Animator.StringToHash("CharacterLeftAttackAnim");
    protected int rightAttackAnimHash = Animator.StringToHash("CharacterRightAttackAnim"); 
    protected int walkBlendTreeHash = Animator.StringToHash("CharacterWalkBlend");
    
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
    }
     
}
