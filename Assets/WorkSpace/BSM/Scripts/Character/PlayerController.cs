using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDir;
 
    [Header("캐릭터 애니메이션 에셋")]
    public SpriteLibraryAsset SpriteLibraryAsset;
 
    [Header("캐릭터 부위")]
    public List<SpriteRenderer> PlayerSprites;
    public Rigidbody2D CharacterRb => characterRb;
    public PlayerModel PlayerModel => playerModel;
    public Animator CharacterAnimator => characterAnimator;
    public SpriteRenderer BodyRenderer;
    public SpriteRenderer HairRenderer;
    
    
    private PlayerState[] characterStates = new PlayerState[(int)CharacterStateType.SIZE];
    private PlayerModel playerModel;
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
        playerModel = GetComponent<PlayerModel>();
        characterRb = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<Animator>();
        
        
        characterStates[(int)CharacterStateType.IDLE] = new PlayerIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new PlayerWalk(this);
    }

    /// <summary>
    /// 캐릭터 키 입력 동작
    /// </summary>
    private void KeyInput()
    {
        posX = Input.GetAxisRaw("Horizontal");
        posY = Input.GetAxisRaw("Vertical");

        if (posX != 0)
        {
            posY = 0;
            BodyRenderer.flipX = posX < 0;
            HairRenderer.flipX = posX < 0;
        }

        if (posY != 0)
        {
            posX = 0;
        }
        
        moveDir = new Vector2(posX, posY);
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

}
