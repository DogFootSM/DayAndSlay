using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : CharacterStateMachine
{
    protected CharacterController characterController;
    
    protected int idleAnimHash = Animator.StringToHash("CharacterIdleAnim");
    protected int walkBlendTreeHash = Animator.StringToHash("CharacterWalkBlend");
    
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    public CharacterState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
     
}
