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

    [Inject] private SqlManager sqlManager;
    [Inject] private DataManager dataManager;
    [Inject] private TableManager tableManager;
    
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private SkillTree curSkillTree;
    [SerializeField] private InventoryInteraction inventoryInteraction;
    [SerializeField] private PlayerSkillReceiver PlayerSkillReceiver;
    
    public Rigidbody2D CharacterRb => characterRb;
    public PlayerModel PlayerModel => playerModel;
    public Animator BodyAnimator => bodyAnimator;
    public Weapon CurWeapon => curWeapon;
    public SkillSlotInvoker SkillSlotInvoker => skillSlotInvoker;
    public CharacterWeaponType CurrentWeaponType => curWeaponType;
    
    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private PlayerState[] characterStates = new PlayerState[(int)CharacterStateType.SIZE];
    private PlayerModel playerModel;
    private Rigidbody2D characterRb;
    private Animator bodyAnimator;
    private Weapon curWeapon;
    private IDataReader dataReader;
    private SkillSlotInvoker skillSlotInvoker; 
    private Table tableObject;

    private LayerMask tableMask;
    private CharacterWeaponType curWeaponType;
    private CharacterStateType curState = CharacterStateType.IDLE;
     
    private int curSlotId => dataManager.SlotId;

    private float posX;
    private float posY;

    private void Awake()
    {
        // ProjectContext.Instance.Container.Inject(this);
        Init(); 
        // InitSlotData();
        // ChangedWeaponType(curWeaponType);
        characterStates[(int)curState].Enter();
        tableMask = LayerMask.NameToLayer("Table");
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
        InventoryToTableItem();
        
        Debug.DrawRay(transform.position, MoveDir * 10f, Color.red);
        
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
        characterStates[(int)CharacterStateType.HIT] = new PlayerHit(this);
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
        quickSlotManager.UpdateWeaponType(curWeaponType);
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
    /// 보유중인 아이템을 테이블 오브젝트에 등록하기 위한 테이블과의 상호작용
    /// </summary>
    private void InventoryToTableItem()
    {
        if (Input.GetKeyDown(KeyCode.E) && tableObject != null)
        { 
            if (tableObject.CurItemDataData == null)
            {
                tableManager.OpenRegisterItemPanel(inventoryInteraction, tableObject);
            }
            else
            {
                tableManager.WithdrawItem(inventoryInteraction, tableObject);
            }
        } 
    }

    /// <summary>
    /// 캐릭터 피격 시 데미지 호출
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(IEffectReceiver monsterReceiver, float damage)
    {
        //TODO: 보호막 쉴드에 따른 데미지 계산ㄱㄱ
        playerModel.ShieldCount--;

        if (playerModel.IsCountering)
        {
            PlayerSkillReceiver.MonsterCounterEvent?.Invoke(monsterReceiver);
        }
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == tableMask)
        {
            tableObject = collision.gameObject.GetComponent<Table>();
        } 
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == tableMask)
        {
            tableManager.OnPlayerExitRangeClosePanel();
            tableObject = null;
        } 
    } 
}