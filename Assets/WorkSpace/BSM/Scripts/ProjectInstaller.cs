using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public DataManager DataManager;
    public MapManager MapManager;
    public SaveManager SaveManager;
    public TableManager TableManager;
    public PlayerContext PlayerContext;
    
    public override void InstallBindings()
    {
        Container.Bind<DataManager>().FromComponentInNewPrefab(DataManager).AsSingle().NonLazy();
        Container.Bind<MapManager>().FromComponentInNewPrefab(MapManager).AsSingle().NonLazy();
        Container.Bind<SaveManager>().FromComponentInNewPrefab(SaveManager).AsSingle().NonLazy();
        Container.Bind<TableManager>().FromComponentInNewPrefab(TableManager).AsSingle().NonLazy();
        Container.Bind<PlayerContext>().FromComponentInNewPrefab(PlayerContext).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SqlManager>().AsSingle().NonLazy();
    }
}
