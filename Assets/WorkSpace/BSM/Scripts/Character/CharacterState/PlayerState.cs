using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected float attackSpeed;
    
    protected PlayerController playerController;
    
    //Body Idle Hash
    protected int upIdleHash = Animator.StringToHash("UpIdle");
    protected int downIdleHash = Animator.StringToHash("DownIdle");
    protected int leftIdleHash = Animator.StringToHash("LeftIdle");
    protected int rightIdleHash = Animator.StringToHash("RightIdle");
    
    //Body Attack Hash
    protected int downAttackHash = Animator.StringToHash("DownAttack");
    protected int upAttackHash = Animator.StringToHash("UpAttack");
    protected int leftAttackHash = Animator.StringToHash("LeftAttack");
    protected int rightAttackHash = Animator.StringToHash("RightAttack"); 
    
    //Walk BlendTree
    protected int walkBlendTreeHash = Animator.StringToHash("WalkBlend");
    
    //BlendTree X,Y 값
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    //TODO: 장착 무기 타입에 따라 공격 해시값 변경 필요
    
    //protected int swordUpAttackAnimHash = Animator.StringToHash("CharacterUpAttackAnim")
    //upAttackAnimHash = swordUpAttackAnimHash;
    
    
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController;

        attackSpeed = playerController.PlayerModel.AtkSpeed;
    }
     
}
