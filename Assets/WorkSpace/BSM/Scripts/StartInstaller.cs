using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartInstaller : MonoInstaller
{
    public CanvasManager UICanvasManager;

    public override void InstallBindings()
    {
        Container.Bind<CanvasManager>().FromInstance(UICanvasManager);
    }
    
}
