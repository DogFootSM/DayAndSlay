using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonsterInstaller : MonoInstaller
{
    [SerializeField] private List<GameObject> monsters;
    [SerializeField] private List<GameObject> bossMonsters;
    [SerializeField] private DungeonPathfinder dfs;
    //[SerializeField] PlayerController player;
    [SerializeField] private TestPlayer player;
    [SerializeField] private DungeonManager dungeonManager;

    public override void InstallBindings()
    {
        Container
            .Bind<List<GameObject>>().WithId("MONSTER")
            .FromInstance(monsters);
        Container
            .Bind<List<GameObject>>().WithId("BOSS")
            .FromInstance(bossMonsters);


        Container
            .Bind<DungeonPathfinder>()
            .FromInstance(dfs);

        //Container
        //    .Bind<PlayerController>()
        //    .FromInstance(player);

        Container
            .Bind<TestPlayer>()
            .FromInstance(player);

        Container
            .Bind<DungeonManager>()
            .FromInstance(dungeonManager);

    }
    
}
