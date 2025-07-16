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

    private Table tableWithItem;
    public Vector3? TargetPos => tableWithItem ? tableWithItem.transform.position : null;

    private Coroutine moveCoroutine;
    private float moveSpeed = 3f;
    private bool isMoving;

    private void Start()
    {
        Init();
        SetupItemWish();
        StateMachine.ChangeState(new NpcIdleState(this));
    }

    private void Update()
    {
        StateMachine.Tick();
    }

    private void Init()
    {
        targetSensor.Init(this);
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
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCoroutine(targetPos));
    }

    private IEnumerator MoveCoroutine(Vector3 target)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, target) > 0.1f)
        {
            Vector3 dir = (target - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        StateMachine.ChangeState(new NpcIdleState(this));
    }

    public void TalkToPlayer()
    {
        talkPopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"{wantItem.name}을 구매하고 싶은데..\n매물이 있을까요?";
        talkPopUp.gameObject.SetActive(true);
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
}


