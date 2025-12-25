using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Npc : MonoBehaviour
{
    
    private Rigidbody2D rb;
    [Inject] public WantItemManager wantItemManager;
    [Inject] private PlayerContext playerContext;
    [Inject] private StoreManager storeManager;
    private PlayerController player;

    [SerializeField] private float angryTime;

    [SerializeField] private GenderType gender;
    [SerializeField] private AgeType age;
    
    public bool IsBuyer;

    [SerializeField] private Animator animator;

    public ItemData wantItem;
    [SerializeField] private List<ItemData> wantItemList;
    [SerializeField] private TargetSensorInNpc targetSensor;
    [SerializeField] private AstarPath astarPath;

    private Table tableWithItem;
    public Vector3? TargetPos => tableWithItem ? tableWithItem.transform.position : null;

    private Coroutine moveCoroutine;
    private float moveSpeed;

    private string currentAnim = "";

    [SerializeField] private bool isAngry;
    private Coroutine blockCoroutine;
    public NpcStateMachine StateMachine { get; private set; } = new();
    private INpcState prevState;
    private Collider2D npcCol;
    private Collider2D playerCol;
    private bool isIgnoringCollision = false;
    
    public bool isSearchTableEnteredFirst = false;
    public string wantItemName;

    /// <summary>
    /// TargetSensorInNpc 따오기
    /// </summary>
    /// <returns></returns>
    public TargetSensorInNpc GetSensor() => targetSensor;

    /// <summary>
    /// StoreManager 따오기
    /// </summary>
    /// <returns></returns>
    public StoreManager GetStoreManager() => storeManager;
    /// <summary>
    /// Target이 될 테이블 설정하기
    /// </summary>
    /// <param name="table"></param>
    public void SetTargetTable(Table table) => tableWithItem = table;
    /// <summary>
    /// Npc 불만족 상태로 변경
    /// </summary>
    public void HeIsAngry() => isAngry = true;
    /// <summary>
    /// Npc가 불만족 상태인지 확인
    /// </summary>
    /// <returns></returns>
    public bool CheckHeIsAngry() => isAngry;

    public (GenderType, AgeType) GetNpcType() => (gender, age);
    
    private SteamStatAPI[] statArray = new SteamStatAPI[1]{SteamStatAPI.GOLD};

    /// <summary>
    /// True = Outside / False = Store
    /// </summary>
    /// <returns></returns>
    public bool IsInOutsideGrid()
    {
        Grid grid = targetSensor.GetCurrentGrid(transform.position);
        return grid != null && grid.name == "OutsideGrid";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isIgnoringCollision)
        {
            moveSpeed = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetMoveSpeed();
        }
    }

    

    private void Start()
    {
        Init();
        SetupItemWish();
        SetMoveSpeed();
        rb = GetComponent<Rigidbody2D>();
        StateMachine.ChangeState(new NpcDecisionState(this));
        npcCol = GetComponent<Collider2D>();
        playerCol = player.GetComponent<Collider2D>();
    }
    
    /// <summary>
    /// NPC 이동속도 지정
    /// </summary>
    private void SetMoveSpeed()
    {
        moveSpeed = 3 + Random.Range(-0.6f, 0.6f);
    }

    private bool isNight = false;

    public void SetIsNightTrue()
    {
        isNight = true;
    }
    
    private void Update()
    {   
        StateMachine.Tick();

        if (!isNight && DayManager.instance.GetDayOrNight() == DayAndNight.NIGHT)
        {
            isNight = true;
            StateMachine.ChangeState(new NpcLeaveState(this));
        }
    }

    private void Init()
    {
        targetSensor.Init(this);
        UpdateGrid();
        player = playerContext.GetPlayerController();
    }

    private void SetupItemWish()
    {
        if (!IsBuyer) return;
        
        wantItemList = SelectItemListByCleared();
        wantItem = wantItemList[Random.Range(0, wantItemList.Count)];
        wantItemName = wantItem.Name;
    }

    private List<ItemData> SelectItemListByCleared()
    {
        List<ItemData> allItems = ItemDatabaseManager.instance.GetAllEquipItem();
        List<ItemData> filteredList;

        // Stage 3까지 클리어: 1 2 3티어 아이템
        if (DungeonManager.is3StageCleared)
        {
            filteredList = allItems
                .Where(item => item.Tier >= 1 && item.Tier <= 3)
                .ToList();
        }
        // Stage 2까지만 클리어: 1 2티어 아이템
        else if (DungeonManager.is2StageCleared)
        {
            filteredList = allItems
                .Where(item => item.Tier >= 1 && item.Tier <= 2)
                .ToList();
        }
        // Stage 2 미클리어: 1 티어 아이템
        else
        {
            filteredList = allItems
                .Where(item => item.Tier == 1)
                .ToList();
        }
    
        return filteredList;
    }


    public Table SearchTable()
    {
        Table[] tables = FindObjectsOfType<Table>();
        foreach (var table in tables)
        {
            if (table.CurItemData == wantItem)
            {
                return table;
            }
        }
        return null;
    }
    public void StopMove()
    {
        if (moveCoroutine != null)
        {
            animator.Play("Idle");
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public void MoveTo(Vector3 targetPos, System.Action onArrive = null)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
    
        // 새로운 코루틴 시작: SetGrid 후 1프레임 대기 로직 포함
        moveCoroutine = StartCoroutine(MoveToAndDetectCoroutine(targetPos, onArrive));
    }

    private IEnumerator MoveToAndDetectCoroutine(Vector3 targetPos, System.Action onArrive)
    {
        UpdateGrid(); 
    
        // 1프레임 대기
        yield return null; 

        astarPath.DetectTarget(transform.position, targetPos);
    
        yield return MoveCoroutine(targetPos, onArrive);
    }

    
    private IEnumerator MoveCoroutine(Vector3 target, System.Action onArrive)
    {
        List<Vector3> path = new List<Vector3>(astarPath.path);
        
        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("A* 경로 없음, 이동 중단");
            PlayDirectionAnimation(Vector3.zero);
            yield break;
        }
        
        foreach (Vector3 point in path)
        {
            // X축 이동
            while (Mathf.Abs(rb.position.x - point.x) > 0.05f)
            {
                float dirX = Mathf.Sign(point.x - rb.position.x);
                Vector3 moveDir = new Vector3(dirX, 0, 0);
                rb.MovePosition(rb.position + (Vector2)moveDir * moveSpeed * Time.fixedDeltaTime);
                PlayDirectionAnimation(moveDir);
                yield return new WaitForFixedUpdate();
            }

            // Y축 이동
            while (Mathf.Abs(rb.position.y - point.y) > 0.05f)
            {
                float dirY = Mathf.Sign(point.y - rb.position.y);
                Vector3 moveDir = new Vector3(0, dirY, 0);
                rb.MovePosition(rb.position + (Vector2)moveDir * moveSpeed * Time.fixedDeltaTime);
                PlayDirectionAnimation(moveDir);
                yield return new WaitForFixedUpdate();
            }

            rb.MovePosition(new Vector2(point.x, point.y)); // 최종 위치 보정
            yield return new WaitForSeconds(0.05f);
        }

        PlayDirectionAnimation(Vector3.zero);
        onArrive?.Invoke();
    }

    public IEnumerator AngryTimeOutCoroutine()
    {
        yield return new WaitForSeconds(angryTime);
        
        FailBuyItem();
    }
    


    /// <summary>
    /// 2분 30초 동안 원하는 아이템을 구하지 못 할 경우 호출
    /// </summary>
    public void FailBuyItem()
    {
        WantItemMarkOnOff(Emoji.ANGRY);
        WantItemClear();
        HeIsAngry();
        wantItemManager.InActiveWantItem(this);
    }

    [SerializeField] private GameObject buyEffect;


    public void BuyItemFromTable()
    {
        if (tableWithItem != null)
        {
            player.GrantExperience(wantItem.SellPrice);
            IngameManager.instance.SetGold(wantItem.SellPrice);
            AchievementManager.Instance.TriggerAchievement(SteamAchievementAPI._1_FIRSTSALE, new SteamStatAPI[1]{SteamStatAPI.NONE}, 0, SteamStatAPI.NONE);
            AchievementManager.Instance.TriggerAchievement(SteamAchievementAPI._2_FIRSTSTEP, statArray, wantItem.SellPrice, SteamStatAPI.GOLD);

            wantItemManager.InActiveWantItem(this);
            StartCoroutine(LeaveCoroutine());
            // 골드 플레이어에게 지급 로직 필요
        }
    }

    private IEnumerator LeaveCoroutine()
    {
        buyEffect.SetActive(true);
        tableWithItem.CurItemData = null;
        
        yield return new WaitUntil(() => !buyEffect.activeSelf);
        
        StateMachine.ChangeState(new NpcLeaveState(this));
    }

    private void WantItemClear()
    {
        wantItem = null;
    }

    /// <summary>
    /// 상점 떠남
    /// </summary>
    public void LeaveStore()
    {
        Vector3 door = targetSensor.GetLeavePosition();
        wantItemManager.InActiveWantItem(this);
        StateMachine.ChangeState(new NpcMoveState(this, door + new Vector3(0, -2, 0), new NpcGoneState(this)));
    }
    

    /// <summary>
    /// 현재 Npc가 서있는 그리드를 업데이트 해줌
    /// </summary>
    public void UpdateGrid()
    {
        Grid currentGrid = targetSensor.GetCurrentGrid(transform.position);
        if (currentGrid != null)
        {
            astarPath.SetGridAndTilemap(currentGrid);
        }
    }

    private void PlayDirectionAnimation(Vector3 dir)
    {
        string nextAnim = "";

        if (dir.x > 0) nextAnim = "RightMove";
        else if (dir.x < 0) nextAnim = "LeftMove";
        else if (dir.y > 0) nextAnim = "UpMove";
        else if (dir.y < 0) nextAnim = "DownMove";
        else nextAnim = "Idle";

        // 같은 애니메이션 반복 호출 방지
        if (nextAnim != currentAnim || nextAnim == "Idle")
        {
            animator.Play(nextAnim, -1, 0f); // 항상 처음부터 재생
            currentAnim = nextAnim;
        }
    }

    public void WantItemMarkOnOff(Emoji num)
    {
        
        GameObject mark = transform.GetChild((int)num).gameObject;
        mark.SetActive(!mark.activeSelf);
    }
    
    /// <summary>
    /// NPC 떠남
    /// </summary>
    public void NpcGone()
    {
        StartCoroutine(GoneCoroutine());
    }

    private IEnumerator GoneCoroutine()
    {
        yield return new WaitForSeconds(3f);
        
        gameObject.SetActive(false);
    }
    
    public void RigidbodyZero()
    {
        var rb = GetComponent<Rigidbody2D>();
        
        rb.velocity = Vector2.zero;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void RigidBodyUnLocked()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
}


