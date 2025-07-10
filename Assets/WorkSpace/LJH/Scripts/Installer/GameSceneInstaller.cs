using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] GameObject weaponForge;
    [SerializeField] GameObject armorForge;

    [Header("·Îµù¾À")]
    [SerializeField] SceneReference loadingScene;
    [Header("´øÁ¯¾À")]
    [SerializeField] SceneReference dungeonScene;
    [Header("¿ÜºÎ¾À")]
    [SerializeField] SceneReference outsideScene;

    [SerializeField] List<NPC> npc = new List<NPC>();

    [SerializeField] ItemStorage itemManager;

    [SerializeField] GameObject popUp;
    [SerializeField] TestPlayer player;

    public override void InstallBindings()
    {
        Container
            .Bind<GameObject>().WithId("WeaponForge")
            .FromInstance(weaponForge);

        Container
            .Bind<GameObject>().WithId("ArmorForge")
            .FromInstance(armorForge);

        Container
            .Bind<SceneReference>().WithId("LoadingScene")
            .FromInstance(loadingScene);

        Container
            .Bind<SceneReference>().WithId("DungeonScene")
            .FromInstance(dungeonScene);

        Container
            .Bind<SceneReference>().WithId("outSideScene")
            .FromInstance(outsideScene);

        Container
            .Bind<List<NPC>>()
            .FromInstance(npc);

        Container
            .Bind<ItemStorage>()
            .FromInstance(itemManager);

        Container
            .Bind<GameObject>().WithId("PopUp")
            .FromInstance(popUp);

        Container
            .Bind<TestPlayer>()
            .FromInstance (player);
    }
}
