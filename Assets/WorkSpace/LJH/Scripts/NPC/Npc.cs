using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Npc : MonoBehaviour
{
    public bool IsBuyer;
    public NpcStateMachine StateMachine { get; private set; } = new();

    [Inject] private ItemStorage itemManager;
    [Inject] private TestPlayer player;
    [Inject] private StoreManager storeManager;

    [SerializeField] private Animator animator;
    [SerializeField] private List<ItemData> wantItemList = new();
    public ItemData wantItem;
    [SerializeField] private PopUp talkPopUp;
    [SerializeField] private TargetSensorInNpc targetSensor;
    [SerializeField] private AstarPath astarPath;

    private Table tableWithItem;
    public Vector3? TargetPos => tableWithItem ? tableWithItem.transform.position : null;

    private Coroutine moveCoroutine;
    private float moveSpeed = 3f;
    private bool isMoving;

    private string currentAnim = "";

    int testNum = 0;
    //Emoji
    [SerializeField] List<Sprite> extensions;
    [SerializeField] private bool isAngry;

    public TargetSensorInNpc GetSensor() => targetSensor;
    public StoreManager GetStoreManager() => storeManager;
    public void SetTargetTable(Table table) => tableWithItem = table;
    public void HeIsAngry() => isAngry = true;
    public bool CheckHeIsAngry() => isAngry;
    public bool IsInOutsideGrid()
    {
        Grid grid = targetSensor.GetCurrentGrid(transform.position);
        return grid != null && grid.name == "OutsideGrid";
    }

    private void Start()
    {
        Init();
        SetupItemWish();
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
    }

    private void SetupItemWish()
    {
        wantItemList = itemManager.ItemDatas;
        wantItem = wantItemList[Random.Range(0, wantItemList.Count)];
    }

    public void SetMoving(bool moving) => isMoving = moving;
    public bool IsMoving() => isMoving;

    public Table SearchTable()
    {
        Table[] tables = FindObjectsOfType<Table>();
        foreach (var table in tables)
        {
            if (table.CurItemDataData == wantItem)
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
        isMoving = true;
        List<Vector3> path = astarPath.path;

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("A* 경로 없음, 이동 중단");
            isMoving = false;
            PlayDirectionAnimation(Vector3.zero);
            yield break;
        }

        foreach (var point in path)
        {
            while (Mathf.Abs(transform.position.x - point.x) > 0.05f)
            {
                float dirX = Mathf.Sign(point.x - transform.position.x);
                Vector3 moveDir = new Vector3(dirX, 0, 0);
                transform.position += moveDir * moveSpeed * Time.deltaTime;
                PlayDirectionAnimation(moveDir);
                yield return null;
            }

            while (Mathf.Abs(transform.position.y - point.y) > 0.05f)
            {
                float dirY = Mathf.Sign(point.y - transform.position.y);
                Vector3 moveDir = new Vector3(0, dirY, 0);
                transform.position += moveDir * moveSpeed * Time.deltaTime;
                PlayDirectionAnimation(moveDir);
                yield return null;
            }

            transform.position = new Vector3(point.x, point.y, transform.position.z);
            yield return new WaitForSeconds(0.05f);
        }

        isMoving = false;
        PlayDirectionAnimation(Vector3.zero);
        onArrive?.Invoke();
    }

    public void TalkToPlayer()
    {
        talkPopUp.gameObject.SetActive(true);
        if (wantItem != null)
        {
            talkPopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"{wantItem.name}을/를 구매하고 싶은데..\n매물이 있을까요?";
        }
    }

    public void BuyItemFromDesk()
    {
        int gold = wantItem.SellPrice;
        wantItem = null;
    }

    public void BuyItemFromTable()
    {
        if (tableWithItem != null)
        {
            int gold = tableWithItem.curItemData.SellPrice;
            tableWithItem.curItemData = null;
            wantItem = null;
            // 골드 플레이어에게 지급 로직 필요
        }
    }

    public void LeaveStore()
    {
        HeIsAngry();
        Vector3 door = targetSensor.GetLeavePosition();
        StateMachine.ChangeState(new NpcMoveState(this, door));
    }

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
    }

    public void Fishing()
    {
        StateMachine.ChangeState(new NpcFishingState(this));
    }

    public void Logging()
    {
        StateMachine.ChangeState(new NpcLoggingState(this));
    }
}


