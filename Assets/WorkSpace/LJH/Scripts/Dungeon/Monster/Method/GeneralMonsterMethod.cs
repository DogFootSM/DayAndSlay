using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    MonsterData monsterData;
    [Inject]
    DungeonManager dungeonManager;
    public void Move()
    {
        //Todo : 몬스터가 캐릭터에게 이동해야 함
    }

    public void Attack()
    {
        //Todo : 몬스터가 공격해야 함
    }

    public void Die()
    {
        //Todo : 몬스터가 죽어야 함
        DropItem();
    }

    private void DropItem()
    {
        //Todo : 확률에 따른 아이템 드랍 만들어야 함
        ItemData dropItemData = monsterData.dropTable[Random.Range(0, monsterData.dropTable.Count)];

        //풀에서 꺼내주고 
        GameObject dropItem = dungeonManager.pool.GetPool();
        dropItem.GetComponent<Item>().itemData = dropItemData;

        //드랍 아이템 위치 조정
        dropItem.transform.position = transform.position;

        //아이템 먹고나면 리턴풀 해줘야함
        // 어디서 처리?
        //아이템 자체 처리가 좋은가?
    }
}
