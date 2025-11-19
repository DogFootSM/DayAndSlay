using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
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
    [Inject] private PlayerContext playerContext;
    
    [SerializeField] private GameObject quiverObject;
    [SerializeField] private SkillTree curSkillTree;
    [SerializeField] private InventoryInteraction inventoryInteraction;
    [SerializeField] private PlayerSkillReceiver PlayerSkillReceiver;
    [SerializeField] private CharacterAnimatorController characterAnimatorController;
    
    [Header("플레이어 사망 오브젝트 On/Off")]
    [SerializeField] private GameObject _cemeteryObject;
    [SerializeField] private GameObject _bodyObject;

    [Header("텔레포트 이펙트")]
    public ParticleSystem teleportParticle;
    
    public Rigidbody2D CharacterRb => characterRb;
    public PlayerModel PlayerModel => playerModel;
    public Animator BodyAnimator => bodyAnimator;
    public Weapon CurWeapon => curWeapon;
    public SkillSlotInvoker SkillSlotInvoker => skillSlotInvoker;
    public CharacterWeaponType CurrentWeaponType => curWeaponType;
    
    [Header("무기 오브젝트")]
    public GameObject WeaponObject;

    [Header("대시 이펙트 오브젝트")] 
    public GameObject DashObject;
    
    /// <summary>
    /// 회피기 사용 가능 상태 여부
    /// </summary>
    [NonSerialized] public bool CanDodge = true;
    
    [SerializeField] private BuffIconController buffIconController;
    
    private QuickSlotManager quickSlotManager => QuickSlotManager.Instance;
    private ArrowPool arrowPool => ArrowPool.Instance;
    private PlayerState[] characterStates = new PlayerState[(int)CharacterStateType.SIZE];
    private PlayerModel playerModel;
    private Rigidbody2D characterRb;
    private Animator bodyAnimator;
    private Weapon curWeapon;
    private IDataReader dataReader;
    private SkillSlotInvoker skillSlotInvoker; 
    private Table tableObject;
    private StoreManager informationDeskObject;
    private DamageEffect _damageEffect;
    private Coroutine dodgeCoolDownCo;
    
    private LayerMask tableLayerMask;
    private LayerMask informationDeskLayerMask;
    private CharacterWeaponType curWeaponType;
    private CharacterStateType curState = CharacterStateType.IDLE;
     
    private int curSlotId => dataManager.SlotId;

    private float posX;
    private float posY;
    
    private bool isDead = false;

    /// <summary>
    /// 회피기 재사용 대기 시간
    /// </summary>
    private const float DODGE_COOLDOWN = 10f;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        Init(); 
        InitSlotData();
        characterStates[(int)curState].Enter();
        curSkillTree.ChangedWeaponType((WeaponType)curWeaponType);
    }

    private void Start()
    {
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
        tableLayerMask = LayerMask.NameToLayer("Table");
        informationDeskLayerMask = LayerMask.GetMask("InformationDesk");
        
        LastMoveKey = Direction.Down;
         
        playerModel = GetComponent<PlayerModel>();
        characterRb = GetComponent<Rigidbody2D>();
        bodyAnimator = GetComponent<Animator>();
        curWeapon = GetComponentInChildren<Weapon>(); 
        skillSlotInvoker = GetComponent<SkillSlotInvoker>();
        _damageEffect = GetComponentInChildren<DamageEffect>();
        
        characterStates[(int)CharacterStateType.IDLE] = new PlayerIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new PlayerWalk(this);
        characterStates[(int)CharacterStateType.ATTACK] = new PlayerAttack(this);
        characterStates[(int)CharacterStateType.SKILL] = new PlayerSkill(this);
        characterStates[(int)CharacterStateType.DEATH] = new PlayerDeath(this);
        characterStates[(int)CharacterStateType.DODGE] = new PlayerDodge(this);
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
        playerContext.Setup(this);
    }
    
    /// <summary>
    /// 현재 무기 타입을 무기에 전달
    /// </summary>
    /// <param name ="weaponType">변경할 무기 타입</param>
    public void ChangedWeaponType(CharacterWeaponType weaponType, ItemData itemData = null)
    { 
        curWeaponType = weaponType;

        if (weaponType == CharacterWeaponType.BOW)
        {
            arrowPool.SetupArrowPoolOnEquip();
        }
        
        //웨폰 핸들러 변경
        if (itemData != null)
        {
            curWeapon.OnWeaponTypeChanged?.Invoke(curWeaponType, itemData, playerModel); 
        }
        //TODO: 웨폰에 따른 애니메이터 컨트롤러 변경
        //TODO: NotWeapon 처리 필요
        characterAnimatorController.AnimatorChange((int)weaponType, true);
        playerModel.UpdateWeaponType(weaponType);
        
        //웨폰에 따른 스킬 트리 변경
        curSkillTree.ChangedWeaponType((WeaponType)curWeaponType);
        
        //장착 무기가 bow 외엔 화살통 오브젝트 비활성화
        quiverObject.SetActive(curWeaponType == CharacterWeaponType.BOW);
        
        //현재 무기에 따른 퀵슬롯 설정 변경
        quickSlotManager.UpdateWeaponType(curWeaponType);
    }

    /// <summary>
    /// 캐릭터 키 입력 동작
    /// </summary>
    private void KeyInput()
    {
        //캐릭터 이동
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (tableObject != null)
            {
                if (tableObject.CurItemData == null)
                {
                    tableManager.OpenRegisterItemPanel(inventoryInteraction, tableObject);
                }
                else
                {
                    tableManager.WithdrawItem(inventoryInteraction, tableObject);
                }
            } 
            else if (informationDeskObject != null)
            {
                tableManager.OpenRegisterItemPanel(inventoryInteraction, informationDeskObject);
            }
        } 
    }

    /// <summary>
    /// 캐릭터 피격 시 데미지 호출
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(IEffectReceiver monsterReceiver, float damage)
    {
        //TODO: 보호막 쉴드에 따른 데미지 계산
        //TODO: 방어력과 몬스터 데미지 공식 정립한 후 체력 감소 진행 + 받는 피해량 감소 버프 상태 체크

        //보호막 쉴드 존재 시 데미지 피해를 받지 않음
        if (playerModel.ShieldCount > 0)
        {
            playerModel.ShieldCount--;
            return;
        }
        
        //현재 반격이 가능한 상태일 경우 몬스터에게 반격 데미지를 가함
        if (playerModel.IsCountering)
        {
            PlayerSkillReceiver.MonsterCounterEvent?.Invoke(monsterReceiver);
        }
        
        //방어력 감소 비율에 데미지 계산한 값
        float takeDamage = damage - (damage * playerModel.DamageReductionRatio);
        playerModel.CurHp -= takeDamage;
        
        _damageEffect.DamageTextEvent(takeDamage);
        _damageEffect.DamageSkinEffect();

        if (playerModel.CurHp > 1) return;
        //체력이 1 미만으로 떨어졌을 경우 데쓰 상태로 변경
        isDead = true;
        ChangeState(CharacterStateType.DEATH);
        
    }
  
    /// <summary>
    /// 캐릭터 사망
    /// </summary>
    public void PlayerDeath()
    {
        _bodyObject.SetActive(false);
        _cemeteryObject.SetActive(true);
        
        //TODO: Death 로직
    }

    /// <summary>
    /// 캐릭터 부활
    /// </summary>
    public void PlayerResurrection()
    {
        _bodyObject.SetActive(true);
        _cemeteryObject.SetActive(false);
        
        ChangeState(CharacterStateType.IDLE);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == tableLayerMask)
        {
            tableObject = collision.gameObject.GetComponent<Table>();
        } 
        else if ((1 << collision.gameObject.layer & informationDeskLayerMask) != 0)
        {
            informationDeskObject = collision.gameObject.GetComponent<StoreManager>();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == tableLayerMask)
        {
            tableManager.OnPlayerExitRangeClosePanel();
            tableObject = null;
        } 
        else if ((1 << other.gameObject.layer & informationDeskLayerMask) != 0)
        {
            tableManager.OnPlayerExitRangeClosePanel();
            informationDeskObject = null;
        }
    }

    /// <summary>
    /// 회피기 쿨타임 초기화
    /// </summary>
    public void ResetDodgeCoolDown(BuffType buffType)
    {
        if (dodgeCoolDownCo != null)
        {
            StopCoroutine(dodgeCoolDownCo);
            dodgeCoolDownCo = null;
        }

        dodgeCoolDownCo = StartCoroutine(DodgeCoolDownCoroutine(buffType));
    }

    /// <summary>
    /// 버프 스킬 아이콘 쿨타임 초기화
    /// </summary>
    /// <param name="buffType"></param>
    /// <param name="cooldownDuration"></param>
    public void ResetBuffSkillCoolDown(BuffType buffType, float cooldownDuration)
    {
        //TODO: 추후 버프 스킬 사용 시 아이콘 필요하다면 여길 호출
        buffIconController.UseBuff(buffType, DODGE_COOLDOWN);
    }
    
    
    private IEnumerator DodgeCoolDownCoroutine(BuffType buffType)
    {
        float elapsedTime = 0;
        
        buffIconController.UseBuff(buffType, DODGE_COOLDOWN);
        while (elapsedTime < DODGE_COOLDOWN)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CanDodge = true;
    }
}