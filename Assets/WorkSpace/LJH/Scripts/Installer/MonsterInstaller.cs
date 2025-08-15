using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class MonsterInstaller : MonoInstaller
{
    [SerializeField] private List<GameObject> monsters_Stage1;
    [SerializeField] private List<GameObject> monsters_Stage2;
    [SerializeField] private List<GameObject> monsters_Stage3;
    
    [SerializeField] private List<GameObject> bossMonsters_Stage1;
    [SerializeField] private List<GameObject> bossMonsters_Stage2;
    [SerializeField] private List<GameObject> bossMonsters_Stage3;
    
    [SerializeField] private DungeonPathfinder dfs;
    //[SerializeField] PlayerController player;
    [SerializeField] private TestPlayer player;
    [SerializeField] private DungeonManager dungeonManager;

    public override void InstallBindings()
    {
        Container
            .Bind<List<GameObject>>().WithId("MONSTER_1")
            .FromInstance(monsters_Stage1);
        Container
            .Bind<List<GameObject>>().WithId("MONSTER_2")
            .FromInstance(monsters_Stage2);
        Container
            .Bind<List<GameObject>>().WithId("MONSTER_3")
            .FromInstance(monsters_Stage3);
        
        Container
            .Bind<List<GameObject>>().WithId("BOSS_1")
            .FromInstance(bossMonsters_Stage1);
        Container
            .Bind<List<GameObject>>().WithId("BOSS_2")
            .FromInstance(bossMonsters_Stage2);
        Container
            .Bind<List<GameObject>>().WithId("BOSS_3")
            .FromInstance(bossMonsters_Stage3);


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
