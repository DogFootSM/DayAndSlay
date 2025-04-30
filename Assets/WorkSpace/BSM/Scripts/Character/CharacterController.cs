using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDir;
    [NonSerialized] public bool IsDownWalk;
    [NonSerialized] public bool IsUpWalk;
    
    public Rigidbody2D CharacterRb => characterRb;
    public CharacterModel CharacterModel => characterModel;
    public Animator CharacterAnimator => characterAnimator;
    public SpriteRenderer BodyRenderer;
    
    private CharacterState[] characterStates = new CharacterState[(int)CharacterStateType.SIZE];
    private CharacterModel characterModel;
    private Rigidbody2D characterRb;
    private Animator characterAnimator;
    
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
        characterAnimator = GetComponent<Animator>();
        
        
        characterStates[(int)CharacterStateType.IDLE] = new CharacterIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new CharacterWalk(this);
    }

    /// <summary>
    /// 캐릭터 키 입력 동작
    /// </summary>
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
    
    /// <summary>
    /// 캐릭터 상태 변경
    /// </summary>
    /// <param name="newState">새로운 상태</param>
    public void ChangeState(CharacterStateType newState)
    {
        characterStates[(int)curState].Exit();
        curState = newState;
        characterStates[(int)curState].Enter(); 
    }

    /// <summary>
    /// 아래로 걷는 애니메이션 좌우 반전 재생 이벤트
    /// </summary>
    public void WalkDownAnimationEvent()
    {
        IsDownWalk = !IsDownWalk;
        BodyRenderer.flipX = IsDownWalk; 
    }

    /// <summary>
    /// 위로 걷는 애니메이션 좌우 반전 재생 이벤트
    /// </summary>
    public void WalkUpAnimationEvent()
    {
        IsUpWalk = !IsUpWalk;
        BodyRenderer.flipX = IsUpWalk; 
    }
    
}
