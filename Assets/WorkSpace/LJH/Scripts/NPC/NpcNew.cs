using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using Zenject;

public class NpcNew : MonoBehaviour
{
    public bool IsBuyer;
    public NpcStateMachine StateMachine { get; private set; } = new();

    [Inject] private ItemStorage itemManager;
    [Inject] private TestPlayer player;

    [SerializeField] private List<ItemData> wantItemList = new();
    public ItemData wantItem;
    [SerializeField] private PopUp talkPopUp;
    [SerializeField] private TargetSensorNew targetSensor;
    [SerializeField] private AstarPath astarPath;

    private Table tableWithItem;
    public Vector3? TargetPos => tableWithItem ? tableWithItem.transform.position : null;

    private Coroutine moveCoroutine;
    private float moveSpeed = 3f;
    private bool isMoving;
    
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

    public void SearchTable()
    {
        Table[] tables = FindObjectsOfType<Table>();
        foreach (var table in tables)
        {
            if (table.CurItemDataData == wantItem)
            {
                tableWithItem = table;
                StateMachine.ChangeState(new MoveState(this, table.transform.position));
                return;
            }
        }

        // 없으면 플레이어로
        StateMachine.ChangeState(new NpcInteractPlayerState(this));
    }

    public void MoveTo(Vector3 targetPos)
    {
        UpdateGrid();
        
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        astarPath.DetectTarget(transform.position, targetPos);
        moveCoroutine = StartCoroutine(MoveCoroutine(targetPos));
    }

    private IEnumerator MoveCoroutine(Vector3 target)
    {
        isMoving = true;

        var path = astarPath.path;

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("A* 경로 없음, 이동 중단");
            isMoving = false;
            yield break;
        }

        foreach (var point in path)
        {
            // 1. 먼저 X축 이동
            while (Mathf.Abs(transform.position.x - point.x) > 0.05f)
            {
                float dirX = Mathf.Sign(point.x - transform.position.x);
                transform.position += new Vector3(dirX * moveSpeed * Time.deltaTime, 0, 0);
                yield return null;
            }

            // 2. 그 다음 Y축 이동
            while (Mathf.Abs(transform.position.y - point.y) > 0.05f)
            {
                float dirY = Mathf.Sign(point.y - transform.position.y);
                transform.position += new Vector3(0, dirY * moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            // 위치 보정
            transform.position = new Vector3(point.x, point.y, transform.position.z);
            yield return new WaitForSeconds(0.05f);
        }

        isMoving = false;
        StateMachine.ChangeState(new NpcIdleState(this));
    }

    public void TalkToPlayer()
    {
        talkPopUp.gameObject.SetActive(true);
        talkPopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"{wantItem.name}을/를 구매하고 싶은데..\n매물이 있을까요?";
        StateMachine.ChangeState(new WaitItemState(this));
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
        Vector3 door = targetSensor.GetLeavePosition();
        StateMachine.ChangeState(new MoveState(this, door));
    }

    public void UpdateGrid()
    {
        Grid currentGrid = targetSensor.GetCurrentGrid(transform.position);
        if (currentGrid != null)
        {
            astarPath.SetGridAndTilemap(currentGrid);
        }
    }
}


