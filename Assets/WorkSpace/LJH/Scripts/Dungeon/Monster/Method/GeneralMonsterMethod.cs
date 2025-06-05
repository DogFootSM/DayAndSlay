using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    MonsterData monsterData;
    [Inject]
    DungeonManager dungeonManager;

    [SerializeField] BoxCollider2D myCollider;

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

    public void Move()
    {
        //Todo : 몬스터가 캐릭터에게 이동해야 함
    }

    public void Attack()
    {
        //Todo : 몬스터가 공격해야 함
        // 플레이어에게서 데미지 주는 함수 이용
        Debug.Log("몬스터가 공격함");
    }

    public void BeforeAttack()
    {
        myCollider.enabled = true;
    }

    public void AfterAttack() 
    {
        myCollider.enabled = false;
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
