using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Npc : MonoBehaviour
{
    private Rigidbody2D rb;
    [Inject] PlayerContext playerContext;
    [Inject] private StoreManager storeManager;
    private PlayerController player;

    [SerializeField] private GenderType gender;
    [SerializeField] private AgeType age;
    
    public bool IsBuyer;

    [SerializeField] private Animator animator;

    public ItemData wantItem;
    [SerializeField] private List<ItemData> wantItemList;
    [SerializeField] private PopUp talkPopUp;
    [SerializeField] private TargetSensorInNpc targetSensor;
    [SerializeField] private AstarPath astarPath;

    private Table tableWithItem;
    public Vector3? TargetPos => tableWithItem ? tableWithItem.transform.position : null;

    private Coroutine moveCoroutine;
    private float moveSpeed = 3f;

    private string currentAnim = "";

    [SerializeField] private bool isAngry;
    private Coroutine blockCoroutine;
    public NpcStateMachine StateMachine { get; private set; } = new();

    /// <summary>
    /// TargetSeneorInNpc 따오기
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

    public void SetNpcType(GenderType gender, AgeType age)
    {
        this.gender = gender;
        this.age = age;
    }

    public (GenderType, AgeType) GetNpcType() => (gender, age);

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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("온콜리전엔터 실행됨 + 플레이어 인식됨");
            PauseMovement();

            if (blockCoroutine == null)
            {
                blockCoroutine = StartCoroutine(BlockNPCCoroutine());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Physics2D.GetIgnoreLayerCollision(15, 20))
            {
                StartCoroutine(ResumeNPCCoroutine(3f));
            }

            else
            {
                StartCoroutine(ResumeNPCCoroutine(0f));
            }
        }
    }

    private IEnumerator BlockNPCCoroutine()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("충돌 판정 무시로 변경");
        Physics2D.IgnoreLayerCollision(20, 15, true);
        blockCoroutine = null;

        // 이동 재개
        StateMachine.ChangeState(new NpcDecisionInStoreState(this, storeManager, targetSensor));
        //State를 기억해뒀다가 전에 진행하던 State로 바꿔줘야함
        INpcState curState = StateMachine.CurrentState;
        StateMachine.ChangeState(curState);

    }

    private IEnumerator ResumeNPCCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Debug.Log("충돌판정 복구");
        if (blockCoroutine != null)
        {
            StopCoroutine(blockCoroutine);
            blockCoroutine = null;
        }

        Physics2D.IgnoreLayerCollision(20, 15, false);

        // 즉시 이동 재개
        StateMachine.ChangeState(new NpcDecisionInStoreState(this, storeManager, targetSensor));
    }
    
    public void PauseMovement()
    {
        if (moveCoroutine != null)
        {
            animator.Play("Idle");
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    private void Start()
    {
        Init();
        SetupItemWish();
        rb = GetComponent<Rigidbody2D>();
        StateMachine.ChangeState(new NpcDecisionState(this));
    }

    private void Update()
    {
        StateMachine.Tick();
    }

    private void Init()
    {
        targetSensor.Init(this);
        UpdateGrid();
        player = playerContext.GetPlayerController();
    }

    private void SetupItemWish()
    {
        wantItemList = ItemDatabaseManager.instance.GetAllEquipItem();
        wantItem = wantItemList[Random.Range(0, wantItemList.Count)];
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
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public void MoveTo(Vector3 targetPos, System.Action onArrive = null)
    {
        UpdateGrid();

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        astarPath.DetectTarget(transform.position, targetPos);
        moveCoroutine = StartCoroutine(MoveCoroutine(targetPos, onArrive));
    }

    
    private IEnumerator MoveCoroutine(Vector3 target, System.Action onArrive)
    {
        List<Vector3> path = astarPath.path;

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

    /// <summary>
    /// 느낌표 뜬 엔피씨가 있을때 플레이어가 호출할 함수
    /// </summary>
    public void TalkToPlayer()
    {
        talkPopUp.gameObject.SetActive(true);
        if (wantItem != null)
        {
            talkPopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"{wantItem.name}을/를 구매하고 싶은데..\n매물이 있을까요?";
        }
    }

    /// <summary>
    /// 대화 이후 호출할 함수
    /// </summary>
    public void AcceptTransaction()
    {
        StateMachine.ChangeState(new NpcWaitItemState(this));
    }
    /// <summary>
    /// 거래 수락시에 호출할 함수
    /// </summary>
    public void TalkExit()
    {
        talkPopUp.gameObject?.SetActive(false);
    }

    public void FailBuyItem()
    {
        WantItemClear();
        HeIsAngry();
    }

    /// <summary>
    /// 아이템 판매 후 처리용 함수
    /// </summary>
    public void BuyItemFromDesk()
    {
        //player.GrantExperience(wantItem.SellPrice);
        WantItemClear();
        WantItemMarkOnOff(Emoji.EXCLAMATION);
        GetStoreManager().PlusRepu(10);
        StateMachine.ChangeState(new NpcLeaveState(this));
    }

    public void BuyItemFromTable()
    {
        if (tableWithItem != null)
        {
            //player.GrantExperience(wantItem.SellPrice);
            tableWithItem.CurItemData = null;
            WantItemClear();
            GetStoreManager().PlusRepu(10);
            StateMachine.ChangeState(new NpcLeaveState(this));
            // 골드 플레이어에게 지급 로직 필요
        }
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
        StateMachine.ChangeState(new NpcMoveState(this, door));
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
        if (nextAnim != currentAnim)
        {
            animator.Play(nextAnim);
            currentAnim = nextAnim;
        }
    }

    public void WantItemMarkOnOff(Emoji num)
    {
        GameObject mark = transform.GetChild((int)num).gameObject;
        mark.SetActive(!mark.activeSelf);
    }


    public bool ArrivedDesk()
    {
        return Vector3.Distance(transform.position, targetSensor.GetDeskPosition()) < 0.5f;
    }

    public void TestCoroutine()
    {
        StartCoroutine(TestCo());
    }

    private IEnumerator TestCo()
    {
        yield return new WaitForSeconds(3f);
        TalkToPlayer();
        StateMachine.ChangeState(new NpcWaitItemState(this));
        yield return new WaitForSeconds(2f);
        TalkExit();
    }

    public void Fishing()
    {
        StateMachine.ChangeState(new NpcFishingState(this));
    }

    public void Logging()
    {
        StateMachine.ChangeState(new NpcLoggingState(this));
    }

    public void NpcGone()
    {
        WantItemMarkOnOff(Emoji.ANGRY);
        gameObject.SetActive(false);
    }
}


