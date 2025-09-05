using UnityEngine;

public class BossMonsterSpawner : MonsterSpawner
{
    public override void MonsterSpawnPosSet()
    {
        //보스방의 경우 스폰 위치가 고정이라 해당 함수 비워둬서 Skip
    }

    /// <summary>
    /// Stage1 >> 미노타우르스, Stage2 >> 벨루스, 말루스, Stage3 >>밴시
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetBossMonster(int index)
    {
        return monsterList[index];
    }

}
