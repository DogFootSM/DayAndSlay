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
    [SerializeField] private SystemWindowController systemWindowController;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private SpriteRenderer hairSpriteRenderer;
    
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

    public Coroutine DodgeCo;
    public Coroutine ParryingCo;
    
    [Header("무기 오브젝트")]
    public GameObject WeaponObject;

    [Header("대시 이펙트 오브젝트")] 
    public GameObject DashObject;
    
    /// <summary>
    /// 회피기 사용 가능 상태 여부
    /// </summary>
    [NonSerialized] public bool CanDodge = true;

    /// <summary>
    /// 패링 상태
    /// </summary>
    [NonSerialized] public bool IsParrying;
    
    /// <summary>
    /// 패링 사용 가능 상태 여부
    /// </summary>
    [NonSerialized] public bool CanParrying = true;
    
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
    private Coroutine parryingCoolDownCo;
    private BoxCollider2D boxCollider;
    
    private LayerMask tableLayerMask;
    private LayerMask informationDeskLayerMask;
    private CharacterWeaponType curWeaponType;
    private CharacterStateType curState = CharacterStateType.IDLE;
    private WeaponTierType curWeaponTier = WeaponTierType.NONE; 
    
    private int curSlotId => dataManager.SlotId;

    private float posX;
    private float posY;
    
    private bool isDead = false;
    public bool IsDead => isDead;

    public static Action<int> OnChangedDebtState;
    
    private void Awake()
    {
        ProjectContext.Instance.Container.Inject(this);
        Init(); 
        InitSlotData();
        characterStates[(int)curState].Enter();
        curSkillTree.ChangedWeaponType((WeaponType)curWeaponType);
    }

    private void OnEnable()
    {
        OnChangedDebtState += UpdateHairLossByDebt;
    }

    private void OnDisable()
    {
        OnChangedDebtState -= UpdateHairLossByDebt; 
    }
 
    private void Start()
    {
        ChangedWeaponType(curWeaponType);
        UpdateHairLossByDebt(playerModel.Debt);
    }
    
    private void Update()
    {
        if (isDead) return;

        KeyInput();
        characterStates[(int)curState].Update();   
        InventoryToTableItem();

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, MoveDir * 10f, Color.red);
#endif
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
        boxCollider = GetComponent<BoxCollider2D>();
        
        characterStates[(int)CharacterStateType.IDLE] = new PlayerIdle(this);
        characterStates[(int)CharacterStateType.WALK] = new PlayerWalk(this);
        characterStates[(int)CharacterStateType.ATTACK] = new PlayerAttack(this);
        characterStates[(int)CharacterStateType.SKILL] = new PlayerSkill(this);
        characterStates[(int)CharacterStateType.DEATH] = new PlayerDeath(this);
        characterStates[(int)CharacterStateType.DODGE] = new PlayerDodge(this);
        characterStates[(int)CharacterStateType.PARRYING] = new PlayerParrying(this);
    }
    
    /// <summary>
    /// 해당 슬롯 데이터로 캐릭터 초기화
    /// </summary>
    private void InitSlotData()
    {
        dataReader = sqlManager.ReadDataColumn(
            new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TIER),
            },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { curSlotId.ToString() },
            null);

        while (dataReader.Read())
        {
            curWeaponType = (CharacterWeaponType)dataReader.GetInt32(0);
            curWeaponTier = (WeaponTierType)dataReader.GetInt32(1);
        }
        playerContext.Setup(this);
    }
    
    /// <summary>
    /// 현재 무기 타입을 무기에 전달
    /// </summary>
    /// <param name ="weaponType">변경할 무기 타입</param>
    public void ChangedWeaponType(CharacterWeaponType weaponType, ItemData itemData = null)
    { 
        if (weaponType == CharacterWeaponType.BOW)
        {
            arrowPool.SetupArrowPoolOnEquip();
        }
        
        //현재 무기 타입과 새로 장착한 무기가 같지 않은 상태
        if (curWeaponType != weaponType)
        {
            curWeaponType = weaponType;
            curWeaponTier = itemData == null ? WeaponTierType.NONE : (WeaponTierType)itemData.Tier;
            //무기 및 바디 애니메이션 교체
            characterAnimatorController.AnimatorChange((int)curWeaponType, (int)curWeaponTier, true);
        }
        else
        {
            //같은 무기 타입이나 무기의 티어가 다를 때 무기 애니메이션 변경
            if (itemData != null)
            {
                if ((WeaponTierType)itemData.Tier != curWeaponTier)
                {
                    curWeaponTier = (WeaponTierType)itemData.Tier;
                    characterAnimatorController.ChangeWeaponAnimator((int)curWeaponType, (int)curWeaponTier);
                }  
            } 
        }
            
        //웨폰 핸들러 변경
        if (itemData != null)
        {
            curWeapon.OnWeaponTypeChanged?.Invoke(curWeaponType, itemData, playerModel); 
        }
  
        playerModel.UpdateWeaponType(curWeaponType, curWeaponTier);
        
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
    /// History : 2025.12.13
    /// 작성자 : 이재호
    /// KeyCode 스페이스바로 변경
    /// </summary>
    private void InventoryToTableItem()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (isDead) return;

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

        //체력이 1 미만으로 떨어졌을 경우 데쓰 상태로 변경
        if (playerModel.CurHp > 0) return;
        ChangeState(CharacterStateType.DEATH);
    }
  
    /// <summary>
    /// 캐릭터 사망
    /// </summary>
    public void PlayerDeath()
    {
        isDead = true;
        _bodyObject.SetActive(false);
        _cemeteryObject.SetActive(true);
        systemWindowController.CanInputKeyUpdate(isDead);
        gameOverCanvas.SetActive(true);
    }

    /// <summary>
    /// 캐릭터 부활
    /// </summary>
    public void PlayerResurrection()
    {
        isDead = false;
        _bodyObject.SetActive(true);
        _cemeteryObject.SetActive(false);
        playerModel.CurHp = playerModel.MaxHp;
        systemWindowController.CanInputKeyUpdate(isDead);
        gameOverCanvas.SetActive(false);
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
    
        ChangeState(CharacterStateType.IDLE);
        dodgeCoolDownCo = StartCoroutine(DodgeCoolDownCoroutine(buffType));
    }

    /// <summary>
    /// 버프 스킬 아이콘 쿨타임 초기화
    /// </summary>
    /// <param name="buffType"></param>
    /// <param name="cooldownDuration"></param>
    public void ResetBuffSkillCoolDown(BuffType buffType, float cooldownDuration)
    {
        buffIconController.UseBuff(buffType, cooldownDuration);
    }

    /// <summary>
    /// 캐릭터 점프 시 Trigger 활성화
    /// 착지 시 Trigger 해제
    /// </summary>
    public void PlayerJumpTrigger()
    {
        boxCollider.isTrigger = !boxCollider.isTrigger;
    }
    
    private IEnumerator DodgeCoolDownCoroutine(BuffType buffType)
    {
        float elapsedTime = 0;
        
        float coolDown = buffType switch
        {
            BuffType.BACKDASH => playerModel.DodgeCooldown,
            BuffType.TELEPORT => playerModel.TeleportCoolDown,
            _ => playerModel.DodgeCooldown
        };

        buffIconController.UseBuff(buffType, coolDown);
 
        while (elapsedTime < coolDown)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CanDodge = true;

        if (DodgeCo != null)
        {
            StopCoroutine(DodgeCo);
            DodgeCo = null;
        } 
    }

    public void ResetParryingCoolDown(BuffType buffType)
    {
        if (parryingCoolDownCo != null)
        {
            StopCoroutine(parryingCoolDownCo);
            parryingCoolDownCo = null;
        }

        ChangeState(CharacterStateType.IDLE);
        parryingCoolDownCo = StartCoroutine(ParryingCoolDownCoroutine(buffType));
    }

    private IEnumerator ParryingCoolDownCoroutine(BuffType buffType)
    {
        float elapsedTime = 0;
        
        buffIconController.UseBuff(buffType, playerModel.ParryingCooldown);
        while (elapsedTime < playerModel.ParryingCooldown)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CanParrying = true;

        if (ParryingCo != null)
        {
            StopCoroutine(ParryingCo);
            ParryingCo = null;
        } 
    } 
    
    /// <summary>
    /// 현재 나의 빚 상황에 따른 탈모 진행 or 탈모 복구
    /// </summary>
    /// <param name="debt"></param>
    private void UpdateHairLossByDebt(int debt)
    {
        float skinHair = debt == 0 ? 1f : ((float)debt / 1000000) > 1f ? 0f : 1 - (float)debt / 1000000;
         
        hairSpriteRenderer.color = new Color(1f, 1f, 1f, skinHair);
    }
    
    /// <summary>
    /// 캐릭터 회복
    /// 이재호가 추가한 메서드
    /// </summary>
    /// <param name="heal"></param>
    public void PlayerHealing(float heal)
    {
        if (isDead) return;
        
        if (playerModel.MaxHp <= playerModel.CurHp + heal)
        {
            playerModel.CurHp =  playerModel.MaxHp;
            return;
        }
        
        playerModel.CurHp += heal;
        
        //체력회복도 회복량 띄우면 좋을거같지만 일단 Todo
        //_damageEffect.DamageTextEvent(heal);
        //_damageEffect.DamageSkinEffect();
    }
}