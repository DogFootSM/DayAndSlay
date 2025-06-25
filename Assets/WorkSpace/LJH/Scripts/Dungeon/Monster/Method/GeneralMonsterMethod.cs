using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    MonsterData monsterData;
    [Inject]
    DungeonManager dungeonManager;

    [SerializeField] BoxCollider2D attackHitBox;
    [SerializeField] AstarPath astarPath;
    [SerializeField] TestPlayer player;

    public Coroutine moveCoroutine;
    public bool isMoving;
    public bool isAttacking = false;

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

    IEnumerator MoveCoroutine()
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
                    transform.position = Vector2.MoveTowards(transform.position, nextPos, monsterData.MoveSpeed * Time.deltaTime);
                    yield return null;
                }

                transform.position = nextPos; // 위치 스냅
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


    public void Die()
    {
        DropItem();
        //경험치 넘김
        //몬스터 죽음

    }

    private void DropItem()
    {
        // 확률 기반으로 랜덤 드랍
        //DropItemEntry dropEntry = monsterData.DropTable[Random.Range(0, monsterData.DropTable.Count)];
        //
        //// ID로 아이템 데이터 불러오기
        //ItemData itemData = ItemManager.GetItemById(dropEntry.ItemId);
        //if (itemData == null)
        //{
        //    Debug.LogWarning("드랍할 아이템을 찾을 수 없습니다.");
        //    return;
        //}
        //
        //GameObject dropItem = dungeonManager.pool.GetPool();
        //dropItem.GetComponent<Item>().itemData = itemData;
        //dropItem.transform.position = transform.position;

        // 이후: 드랍 아이템이 일정 시간 뒤에 자동으로 풀에 리턴되게 만들기
    }
}
