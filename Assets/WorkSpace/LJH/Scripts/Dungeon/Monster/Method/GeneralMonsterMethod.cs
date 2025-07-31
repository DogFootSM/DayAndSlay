using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    MonsterData monsterData;
    [Inject]
    DungeonManager dungeonManager;
    [Inject]
    ItemStorage itemStorage;

    [SerializeField] protected BoxCollider2D attackHitBox;
    [SerializeField] protected AstarPath astarPath;
    [SerializeField] protected GameObject player;
    protected MonsterModel monster;

    public Coroutine moveCoroutine;
    public bool isMoving;
    public bool isAttacking = false;

    private void Start()
    {
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
                        break; // 목표에 도달
                    }

                    Vector3 nextPos = current + moveDir * monsterData.MoveSpeed * Time.deltaTime;

                    // Clamp: 목표를 넘어가지 않도록
                    if (Vector2.Distance(nextPos, target) > Vector2.Distance(current, target))
                    {
                        nextPos = target;
                    }

                    transform.position = nextPos;

                    yield return null;
                }

                transform.position = target; // 위치 스냅
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

        //Todo : 코루틴 제어 정상적으로 돌아가게 해줘야함
        // 이동 시작할때 무브코에 넣어주고 스타트 한다음 공격 상태가 된 경우 or 플레이어가 이동하여 경로 재탐색 하는 경우에 스탑코루틴 해주면서 moveCo 널로 바꿔주면 될듯
        // 이동 구현되면 아이템 드랍 구현해야됨
        // 아이템 풀을 이용해서 구현할 것
        // 현재 이슈 발생
        // >> 플레이어가 밖에서 추격 범위로 들어갔을 때 코루틴 널 뜸
        // 코루틴 관련해서 만져주면 될듯
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
        ItemData itemData = itemStorage.GetItemById(itemStorage.IngrediantDict, ItemId);

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
