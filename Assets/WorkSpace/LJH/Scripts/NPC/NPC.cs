using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NPC : MonoBehaviour
{
    public bool IsBuyer;

    [Inject] private ItemStorage itemManager;

    [SerializeField] private List<ItemData> wantItemList = new List<ItemData>();

    [SerializeField] private ItemData wantItem;
    [SerializeField] private List<Table> tables = new List<Table>();

    //이동 관련
    private bool isMoving;
    private AstarPath astarPath;
    Coroutine moveCoroutine;
    int moveSpeed = 3;

    private Table table;
    public Table _table => table;

    private void Start()
    {
        Init();
        NpcBehaviour();
    }


    private void NpcBehaviour()
    {
        if (!IsBuyer)
        {
            DontGoStore();
            return;
        }

        ItemListSetting(itemManager.ItemDatas);
        PickItem();
        GoStore();
    }

    /// <summary>
    /// 아이템매니저에서 현재 판매 가능한 아이템 리스트 넣어줌
    /// </summary>
    /// <param name="itemList"></param>
    private void ItemListSetting(List<ItemData> itemList)
    {
        wantItemList = itemList;
    }


    /// <summary>
    /// 어떤 아이템을 원하는지 설정
    /// </summary>
    private void PickItem()
    {
        int itemIndex = Random.Range(0, wantItemList.Count);
        //wantItem = wantItemList[itemIndex];
        wantItem = wantItemList[0];
    }


    private void GoStore()
    {
        //Todo : 상점으로 이동하는 엔피씨
        //상점 내부로 이동해야함
        //입구에 서있어야함
        //원하는 아이템이 테이블에 있을 경우 테이블에 가서 아이템을 구매함(테이블에 있는 아이템 사라지고 플레이어에게 돈을 줌
        //원하는 아이템이 테이블에 없을 경우 플레이어에게 말을 검(느낌표 띄우고 대기로 생각중)
        //가서 말걸면 "낡은 검을 사고 싶어요" 말함
        //아이템 만들어와서 말걸면 
        // 아이템 만들어옴 > 아이템 판매
        // 아이템 없음 > 엔피씨 대기하다가 나감

        //테스트끝나면 인보크 삭제
        Invoke("NPCMove", 3f);

    }

    private void DontGoStore()
    {
        //Todo : 상점으로 가지 않는 엔피씨
        //상점 바깥 맵에서 이동해야함
    }

    private void InStoreBehaviour()
    {
        TableScan();
    }

    /// <summary>
    /// 상점맵 > 상점맵에서 테이블 탐색
    /// </summary>
    private void TableScan()
    {
        Table[] tableArray = FindObjectsOfType<Table>();
        tables = new List<Table>(tableArray);

        WantItemCheck();
    }
    /// <summary>
    /// 상점맵 > 상점맵에서 구매할 아이템 있는지 체크
    /// </summary>
    private void WantItemCheck()
    {
        for(int i = 0; i < tables.Count; i++)
        {
            if(wantItem == tables[i].CurItemDataData)
            {
                //테이블에 아이템이 있는 경우 테이블로 이동하여 아이템 구매
                table = tables[i];
            }
        }
    }

    private void NPCMove()
    {
        moveCoroutine = StartCoroutine(MoveCoroutine());
        Debug.Log("무브 코루틴 실행");
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        if (astarPath.path != null && astarPath.path.Count > 1)
        {
            for (int i = 1; i < astarPath.path.Count; i++)
            {
                Vector3 current = transform.position;
                Vector3 target = astarPath.path[i];

                Vector3 direction = target - current;
                Vector3 moveDir;

                // 대각선 방지: x 또는 y 중 큰 쪽만 이동
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    moveDir = (direction.x > 0) ? Vector3.right : Vector3.left;
                else
                    moveDir = (direction.y > 0) ? Vector3.up : Vector3.down;

                Vector3 nextPos = current + moveDir;

                // 타일 한 칸씩 이동
                while (Vector2.Distance(transform.position, nextPos) > 0.01f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = nextPos; // 위치 스냅
                yield return new WaitForSeconds(0.05f);
            }
        }

        isMoving = false;
        moveCoroutine = null;
    }

    private void Init()
    {
        astarPath = transform.GetComponentInChildren<AstarPath>();
    }
}
