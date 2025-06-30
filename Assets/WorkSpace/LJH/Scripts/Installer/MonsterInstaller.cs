using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonsterInstaller : MonoInstaller
{
    [SerializeField] List<GameObject> monsters;
    [SerializeField] DungeonPathfinder dfs;
    //[SerializeField] PlayerController player;
    [SerializeField] TestPlayer player;
    [SerializeField] DungeonManager dungeonManager;
    [SerializeField] ItemStorage itemStorage;

    public override void InstallBindings()
    {
        Container
            .Bind<List<GameObject>>()
            .FromInstance(monsters);


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

        Container
            .Bind<ItemStorage>()
            .FromInstance(itemStorage);
    }
    
}
