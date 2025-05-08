using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonsterInstaller : MonoInstaller
{
    [SerializeField] List<GameObject> monsters;

    public override void InstallBindings()
    {
        Container
            .Bind<List<GameObject>>()
            .FromInstance(monsters);
    }
    
}
