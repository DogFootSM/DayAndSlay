using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartInstaller : MonoInstaller
{
    public CanvasManager UICanvasManager;
    public DataManager DataManager;
    
    
    public override void InstallBindings()
    {
        Container.Bind<CanvasManager>().FromInstance(UICanvasManager);
        Container.Bind<DataManager>().FromInstance(DataManager);
    }
    
}
