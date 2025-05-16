using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public DataManager DataManager;
    public WaitCache WaitCache;  
    
    public override void InstallBindings()
    {
        Container.Bind<DataManager>().FromComponentInNewPrefab(DataManager).AsSingle().NonLazy();
        Container.Bind<WaitCache>().FromInstance(WaitCache);
        Container.BindInterfacesAndSelfTo<SqlManager>().AsSingle().NonLazy();
    }
}
