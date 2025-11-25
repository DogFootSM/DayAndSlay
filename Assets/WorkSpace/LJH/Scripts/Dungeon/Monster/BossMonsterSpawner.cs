using UnityEngine;

public class BossMonsterSpawner : MonsterSpawner
{
    [SerializeField] private GameObject stone;
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
        //스타트 단에서 실행되어야해 여기에 같이 넣어줌
        DungeonManager.Instance.SetStoneInBossDoor(stone);
        
        return monsterList[index];
    }

    protected override void Start()
    {
        base.Start();
        DungeonManager.Instance.SetStoneInBossDoor(stone);
    }

}
