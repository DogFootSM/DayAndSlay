using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector2 MoveDir;
    [NonSerialized] public Direction LastMoveKey;
    
    [Header("무기 애니메이션 컨트롤러 컴포넌트")] public Animator WeaponAnimator;

    public Rigidbody2D CharacterRb => characterRb;
    public PlayerModel PlayerModel => playerModel;
    public Animator BodyAnimator => bodyAnimator;
    public Weapon CurWeapon => curWeapon;
    public SkillSlotInvoker SkillSlotInvoker => skillSlotInvoker;

    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;

    [SerializeField] private GameObject shieldObject;
    [SerializeField] private SkillTree curSkillTree;
    
    private PlayerState[] characterStates = new PlayerState[(int)CharacterStateType.SIZE];
    private PlayerModel playerModel;
    private Rigidbody2D characterRb;
    private Animator bodyAnimator;
    private Weapon curWeapon;
    private IDataReader dataReader;
    private SkillSlotInvoker skillSlotInvoker; 
    
    private CharacterWeaponType curWeaponType;
    private CharacterStateType curState = CharacterStateType.IDLE;
     
    private int curSlotId => dataManager.SlotId;

    private float posX;
    private float posY;


    //이재호가 붙여둔 임시 코드
    private GameObject interactObj;
    private IInteractionStoreScene interactable;



    private void Awake()
    {
        // ProjectContext.Instance.Container.Inject(this);
        Init(); 
        // InitSlotData();
        // ChangedWeaponType(curWeaponType);
        characterStates[(int)curState].Enter();
    }

    private void Start()
    {
        //TODO: 테스트용
        ProjectContext.Instance.Container.Inject(this);
        InitSlotData();
        ChangedWeaponType(curWeaponType);
    }

    private void Update()
    {
        KeyInput();
        characterStates[(int)curState].Update();   
        
        TakeInteraction();
    }

    private void FixedUpdate()
    {
        characterStates[(int)curState].FixedUpdate();
    }

    private void Init()
    {
        LastMoveKey = Direction.Down;
        
        playerModel = GetComponent<PlayerModel>();
        characterRb = GetComponent<Rigidbody2D>();
        bodyAnimator = GetComponent<Animator>();
        curWeapon = GetComponentInChildren<Weapon>(); 
        skillSlotInvoker = GetComponent<SkillSlotInvoker>();
        
        characterStates[(int)CharacterStateType.IDLE] = new PlayerIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new PlayerWalk(this);
        characterStates[(int)CharacterStateType.ATTACK] = new PlayerAttack(this);
        characterStates[(int)CharacterStateType.SKILL] = new PlayerSkill(this);
    }
    
    /// <summary>
    /// 해당 슬롯 데이터로 캐릭터 초기화
    /// </summary>
    private void InitSlotData()
    {
        dataReader = sqlManager.ReadDataColumn(
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE) },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { curSlotId.ToString() },
            null);

        while (dataReader.Read())
        {
            curWeaponType = (CharacterWeaponType)dataReader.GetInt32(0);
        }

    }
    
    /// <summary>
    /// 현재 무기 타입을 무기에 전달
    /// </summary>
    /// <param name ="weaponType">변경할 무기 타입</param>
    public void ChangedWeaponType(CharacterWeaponType weaponType)
    { 
        curWeaponType = weaponType;
        
        curWeapon.OnWeaponTypeChanged?.Invoke(curWeaponType);
        
        curSkillTree.ChangedWeaponType((WeaponType)curWeaponType);
        shieldObject.SetActive(curWeaponType == CharacterWeaponType.SHORT_SWORD);
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
 
        MoveDir = new Vector2(posX, posY);
        
        if (MoveDir != Vector2.zero)
        {
            curWeapon.OnDirectionChanged?.Invoke(new Vector2(posX, posY)); 
            skillSlotInvoker.OnDirectionChanged?.Invoke(new Vector2(posX, posY));
            LastMoveInputKeyCheck();
        }
        
    }
 
    /// <summary>
    /// 마지막 이동 키 입력 확인
    /// </summary>
    private void LastMoveInputKeyCheck()
    {
        if (posY > 0) LastMoveKey = Direction.Up;
        else if(posY < 0) LastMoveKey = Direction.Down;
        else if(posX > 0) LastMoveKey = Direction.Left;
        else if(posX < 0) LastMoveKey = Direction.Right;
        
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
    /// 경험치를 model에게 넘겨줌
    /// </summary>
    /// <param name="exp">아이템을 팔았을 때 받을 경험치</param>
    public void GrantExperience(int exp)
    {
        playerModel.GainExperience(exp);
    }

    /// <summary>
    /// 이재호가 붙인 임시 코드
    /// </summary>
    private void TakeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            interactObj != null)
        {
            Debug.Log("TABLE과 상호작용");
            
            switch (interactable)
            {
                //E 키를 눌렀을 때, TableManager에 아이템 등록 팝업 활성화 요청하면서 인벤토리 넘겨줌
                //TableManager에서 활성화 되면 인벤토리에 들어있는 아이템으로 UI 설정
                //유저가 아이템을 선택하면 Table의 Interaction 호출하면서 선택한 아이템 데이터 넘겨줌
                
                
                case Table table: 
                    //table.TakeItem(choiceItem);
                    break;

                default:
                    interactable.Interaction();
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out interactable))
        {
            interactObj = collision.gameObject;
        }
    }

}