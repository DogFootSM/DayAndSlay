using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : CharacterStateMachine
{
    protected CharacterController characterController;

    public CharacterState(CharacterController characterController)
    {
        this.characterController = characterController;
    }
     
}
