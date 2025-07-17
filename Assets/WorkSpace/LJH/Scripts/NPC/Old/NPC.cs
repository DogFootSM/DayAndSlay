using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class NPC : MonoBehaviour
{
    public bool IsBuyer;

    [Inject] private ItemStorage itemManager;

    public TargetSensorInNPC targetSensor;

    [SerializeField] private List<ItemData> wantItemList = new List<ItemData>();

    public ItemData wantItem;
    [SerializeField] private List<Table> tables = new List<Table>();

    //Seller (player or table)
    GameObject seller;
    [Inject] TestPlayer player;

    //이동 관련
    private bool isMoving;
    private AstarPath astarPath;
    private Coroutine moveCoroutine;
    private int moveSpeed = 3;

    /// <summary>
    /// NPC가 원하는 아이템을 들고있는 테이블
    /// </summary>
    [SerializeField]  private Table tablewithItem;
    public Table _table => tablewithItem;

    //npc의 인내심
    private int patience = 60;

    [SerializeField] PopUp talkPopUp;

    private void Start()
    {
        Init();
        NpcBehaviour();

    }


    private void NpcBehaviour()
    {
        if (IsBuyer)
        {
            ItemListSetting(itemManager.ItemDatas);
            PickItem();
        }
        
        BuyerHaviour();
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
        wantItem = wantItemList[1];
    }


    private void BuyerHaviour()
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
        TableScan();

    }

    /// <summary>
    /// 상점맵 > 상점맵에서 테이블 탐색
    /// </summary>
    private void TableScan()
    {
        Table[] tableArray = FindObjectsOfType<Table>();
        tables = new List<Table>(tableArray);

        for (int i = 0; i < tables.Count; i++)
        {
            if (wantItem == tables[i].CurItemDataData)
            {
                //테이블에 아이템이 있는 경우 테이블로 이동하여 아이템 구매
                tablewithItem = tables[i];

                targetSensor.InjectTable(tablewithItem);
            }
        }
    }

    public void BuyItem(GameObject seller, ItemData item)
    {
        if(seller.GetComponent<Table>())
        {
            Table table = seller.GetComponent<Table>();
            int gold = table.curItemData.SellPrice;

            table.curItemData = null;
            wantItem = null;
            //Todo : 플레이어에게 골드 전달해야함
            
        }
        else
        {
            TalkToPlayer();
        }
    }

    private void TalkToPlayer()
    {
        talkPopUp.GetComponentInChildren<TextMeshProUGUI>().text = $"{wantItem}을 구매하고 싶은데.. \n 매물이 있을까요?";

        talkPopUp.gameObject.SetActive(true);
    }


    public void NPCMove()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;
        yield return new WaitForSeconds(1f);

        if (astarPath.path != null && astarPath.path.Count > 1)
        {
            for (int i = 1; i < astarPath.path.Count; i++)
            {
                Vector3 target = astarPath.path[i];

                while (Vector2.Distance(transform.position, target) > 0.01f)
                {
                    Vector3 current = transform.position;
                    Vector3 direction = target - current;

                    Vector3 moveDir = Vector3.zero;

                    if (Mathf.Abs(direction.x) > 0.01f)
                    {
                        moveDir = (direction.x > 0) ? Vector3.right : Vector3.left;
                    }
                    else if (Mathf.Abs(direction.y) > 0.01f)
                    {
                        moveDir = (direction.y > 0) ? Vector3.up : Vector3.down;
                    }
                    else
                    {
                        SetMoving(false);
                        break; // 목표에 도달
                    }

                    Vector3 nextPos = current + moveDir * moveSpeed * Time.deltaTime;

                    // Clamp: 목표를 넘어가지 않도록
                    if (Vector2.Distance(nextPos, target) > Vector2.Distance(current, target))
                    {
                        nextPos = target;
                    }

                    transform.position = nextPos;

                    yield return null;
                }

                transform.position = target;
                yield return new WaitForSeconds(0.05f);
            }
        }

        isMoving = false;
        moveCoroutine = null;
    }

    public void SetMoving(bool moved)
    {
        isMoving = moved;
    }

    public bool GetMoving() => isMoving;

    private void Init()
    {
        astarPath = transform.GetComponentInChildren<AstarPath>();
        seller = player.gameObject;
    }
}
