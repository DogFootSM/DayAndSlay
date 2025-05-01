using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MonsterInstaller : MonoInstaller
{
    [SerializeField] GameObject testMonster;

    public override void InstallBindings()
    {
        Container
            .Bind<GameObject>()
            .FromInstance(testMonster);
    }
    
}
