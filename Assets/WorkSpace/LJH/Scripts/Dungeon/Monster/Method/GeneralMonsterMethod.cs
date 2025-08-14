using System.Collections;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    private MonsterData monsterData;
    [Inject] private DungeonManager dungeonManager;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected BoxCollider2D attackHitBox;
    [SerializeField] protected AstarPath astarPath;
    [SerializeField] protected GameObject player;
    protected MonsterModel monster;

    public Coroutine moveCoroutine;
    public bool isMoving;
    public bool isAttacking = false;

    private void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        monster = GetComponent<MonsterModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AttackMethod(monsterData.Attack);
        }
    }

    public void MonsterDataInit(MonsterData monsterData)
    {
        this.monsterData = monsterData;

        if (this.monsterData == null)
            Debug.Log("몬스터데이터 주입 안됨");
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        if (astarPath.path != null && astarPath.path.Count > 1)
        {
            for (int i = 1; i < astarPath.path.Count; i++)
            {
                Vector2 target = astarPath.path[i];

                while (Vector2.Distance(rb.position, target) > 0.01f)
                {
                    Vector2 current = rb.position;
                    Vector2 direction = target - current;

                    Vector2 moveDir = Vector2.zero;

                    if (Mathf.Abs(direction.x) > 0.01f)
                    {
                        moveDir = (direction.x > 0) ? Vector2.right : Vector2.left;
                    }
                    else if (Mathf.Abs(direction.y) > 0.01f)
                    {
                        moveDir = (direction.y > 0) ? Vector2.up : Vector2.down;
                    }
                    else
                    {
                        break;
                    }

                    Vector2 nextPos = current + moveDir * monsterData.MoveSpeed * Time.fixedDeltaTime;

                    // Clamp: 목표를 넘어가지 않도록
                    if (Vector2.Distance(nextPos, target) > Vector2.Distance(current, target))
                    {
                        nextPos = target;
                    }

                    rb.MovePosition(nextPos);
                    yield return new WaitForFixedUpdate();
                }

                rb.MovePosition(target); // 목표 위치 스냅
                yield return new WaitForSeconds(0.05f);
            }
        }

        isMoving = false;
        moveCoroutine = null;
    }

    public void StopMoveCo()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
            isMoving = false;
        }
    }

    public void MoveMethod()
    {

        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            moveCoroutine = StartCoroutine(MoveCoroutine());
            
        }
    }

    public void AttackMethod(int damage)
    {
        //int hp = player.GetComponent<PlayerStats>().Health;
        int hp = 100;
        hp -= damage;
        //Todo : 실제 데미지 처리 해야함
    }

    public void HitMethod(int damage)
    {
        Debug.Log("몬스터 맞았음");
        monster.SetMonsterHp(damage);
    }


    public void DieMethod()
    {
        DropItem();       
        //경험치 넘김
        //몬스터 죽음

    }

    private void DropItem()
    {
        //확률 기반으로 랜덤 드랍
        DropItemEntry dropEntry = monsterData.DropTable[Random.Range(0, monsterData.DropTable.Count)];
        int ItemId = dropEntry.ItemId;
        
        // ID로 아이템 데이터 불러오기
        ItemData itemData = ItemDatabaseManager.instance.GetItemByID(ItemId);

        if (itemData == null)
        {
            Debug.LogWarning("드랍할 아이템을 찾을 수 없습니다.");
            return;
        }
        
        GameObject dropItem = dungeonManager.pool.GetPool();
        dropItem.GetComponent<Item>().itemData = itemData;
        dropItem.transform.position = transform.position;

        //이후: 드랍 아이템이 일정 시간 뒤에 자동으로 풀에 리턴되게 만들기
    }
}
