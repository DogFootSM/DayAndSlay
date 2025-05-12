using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector2 moveDir;
    [NonSerialized] public string LastKey = "s";
    
    [Header("캐릭터 애니메이션 에셋 - 1.Hair, 2.Body")]
    public SpriteLibraryAsset[] BodyLibraryAsset;
 
    [Header("캐릭터 장비 애니메이션 에셋 ")]
    public SpriteLibraryAsset[] EquipmentLibraryAsset;
    
    [Header("캐릭터 부위")]
    public List<SpriteRenderer> PlayerSprites;
    public Rigidbody2D CharacterRb => characterRb;
    public PlayerModel PlayerModel => playerModel;
    public Animator BodyAnimator => bodyAnimator;
    public Animator WeaponAnimator;
    public WaitCache WaitCache => waitCache;
    
    
    [Inject] private WaitCache waitCache;
    
    
    private PlayerState[] characterStates = new PlayerState[(int)CharacterStateType.SIZE];
    private PlayerModel playerModel;
    private Rigidbody2D characterRb;
    private Animator bodyAnimator;
    
    private CharacterStateType curState = CharacterStateType.IDLE;
    
    private float posX;
    private float posY;
     
    private const string MoveUp = "MoveUp";
    private const string MoveDown = "MoveDown";
    private const string MoveLeft = "MoveLeft";
    private const string MoveRight = "MoveRight";
    
    private void Awake()
    {
        Init();
        characterStates[(int)curState].Enter(); 
    }

    private void Start()
    {
        ProjectContext.Instance.Container.Inject(this);
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
        bodyAnimator = GetComponent<Animator>();
        
        
        characterStates[(int)CharacterStateType.IDLE] = new PlayerIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new PlayerWalk(this);
        characterStates[(int)CharacterStateType.ATTACK] = new PlayerAttack(this);
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
            
        }
         
        if (posY != 0)
        {
            posX = 0;
        }
        
        if (Input.GetButtonDown(MoveUp)
            || Input.GetButtonDown(MoveDown)
            || Input.GetButtonDown(MoveLeft)
            || Input.GetButtonDown(MoveRight))
        { 
            LastInputKeyCheck();
        }
        
        moveDir = new Vector2(posX, posY); 
    }

    private void LastInputKeyCheck()
    { 
        if (LastKey.Equals(Input.inputString)) return;
        
        StringBuilder sb = new StringBuilder(LastKey);

        sb.Replace(LastKey, Input.inputString);

        LastKey = sb.ToString();
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
