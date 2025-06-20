using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSpawner : MonsterSpawner
{
    public override void MonsterSpawnPosSet()
    {
            Vector3 spawnPos = Vector3.zero;
            Vector3Int tilePos = Vector3Int.zero;

            tilePos = floor.WorldToCell(spawnPos);


        spawnerList[0].transform.position = spawnPos;
    }

    public override void MonsterSpawn()
    {
            //젠젝트로 사용해야 하기에 컨테이너를 이용한 Instantiate 사용
            monsterList.Add(container.InstantiatePrefab
                (monsters[0], spawnerList[0].transform.position, Quaternion.identity, null));

            //초기 생성시 플레이어가 없는 방에서는 몬스터 비활성화
            MonsterActiver();
    }
}
