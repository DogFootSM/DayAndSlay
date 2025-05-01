using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public DataManager DataManager;
    
    public override void InstallBindings()
    {
        Container.Bind<DataManager>().FromInstance(DataManager);
    }
}
