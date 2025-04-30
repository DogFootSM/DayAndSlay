using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDir;

    public Rigidbody2D CharacterRb => characterRb;
    public CharacterModel CharacterModel => characterModel;
    
    private CharacterState[] characterStates = new CharacterState[(int)CharacterStateType.SIZE];
    private CharacterModel characterModel;
    private Rigidbody2D characterRb;
    
    private CharacterStateType curState = CharacterStateType.IDLE;
     
    private float posX;
    private float posY;
    
    
    private void Awake()
    {
        Init();
        characterStates[(int)curState].Enter();
    }

    private void Update()
    {
        KeyInput();
        characterStates[(int)curState].Update(); 
    }

    private void FixedUpdate()
    {
        characterStates[(int)curState].FixedUpdate();
    }
     
    private void Init()
    {
        characterModel = GetComponent<CharacterModel>();
        characterRb = GetComponent<Rigidbody2D>();
        
        characterStates[(int)CharacterStateType.IDLE] = new CharacterIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new CharacterWalk(this);
    }

    private void KeyInput()
    {
        posX = Input.GetAxisRaw("Horizontal");
        posY = Input.GetAxisRaw("Vertical");
        
        moveDir = new Vector2(posX, posY);

        if (moveDir.SqrMagnitude() >= 1f)
        {
            moveDir.Normalize();
        } 
    }
    
    public void ChangeState(CharacterStateType newState)
    {
        characterStates[(int)curState].Exit();
        curState = newState;
        characterStates[(int)curState].Enter(); 
    }
    
}
