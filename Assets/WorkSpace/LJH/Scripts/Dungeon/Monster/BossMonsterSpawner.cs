using UnityEngine;

public class BossMonsterSpawner : MonsterSpawner
{
    public override void MonsterSpawnPosSet()
    {
        //보스방의 경우 스폰 위치가 고정이라 해당 함수 비워둬서 Skip
    }

    public override void MonsterSpawn()
    {
           
    }
}
