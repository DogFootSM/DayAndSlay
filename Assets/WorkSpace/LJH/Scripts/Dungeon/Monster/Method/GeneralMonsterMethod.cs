using System.Collections;
using System.Collections.Generic;
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

    public Coroutine moveCo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Attack();
        }
    }

    public void MonsterDataInit(MonsterData monsterData)
    {
        this.monsterData = monsterData;

        if (this.monsterData == null)
            Debug.Log("몬스터데이터 주입 안됨");
    }

    IEnumerator MoveCo()
    {
        for (int i = 0; i < astarPath.path.Count; i++)
        {
            yield return new WaitForSeconds(0.3f);
            transform.position = astarPath.path[i];
        }
    }

    public void Move()
    {
        //Todo : 몬스터가 캐릭터에게 이동해야 함
        /*
        Vector2Int가 담긴 리스트 갱신 
        Vector2Int.up 이면 위로 한칸 이동함
        Vector2Int가 담긴 리스트 갱신 
        Vector2Ini.right 이면 오른쪽으로 한칸 이동함
        */

        if(moveCo == null) moveCo = StartCoroutine(MoveCo());
        
        //Todo : 코루틴 제어 정상적으로 돌아가게 해줘야함
        // 이동 시작할때 무브코에 넣어주고 스타트 한다음 공격 상태가 된 경우 or 플레이어가 이동하여 경로 재탐색 하는 경우에 스탑코루틴 해주면서 moveCo 널로 바꿔주면 될듯
        // 이동 구현되면 아이템 드랍 구현해야됨
        // 아이템 풀을 이용해서 구현할 것

    }

    public void Attack()
    {
        //Todo : 몬스터가 공격해야 함
        // 플레이어에게서 데미지 주는 함수 이용
        Debug.Log("몬스터가 공격함");
    }

    public void BeforeAttack()
    {
        attackHitBox.enabled = true;
    }

    public void AfterAttack() 
    {
        attackHitBox.enabled = false;
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
