using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMonsterMethod : MonoBehaviour
{
    MonsterData monsterData;
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
    }

    public void DropItem()
    {
        //Todo : 몬스터가 아이템 드랍해야 함
        Item dropItem = monsterData.dropTable[Random.Range(0, monsterData.dropTable.Count)];

        Instantiate(dropItem, transform.position, Quaternion.identity);
    }
}
