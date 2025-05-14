using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class StartInstaller : MonoInstaller
{
    public CanvasManager UICanvasManager;
    
    private SqliteDatabase database;
    
    public override void InstallBindings()
    {
        Container.Bind<CanvasManager>().FromInstance(UICanvasManager);
    }

    /// <summary>
    /// 게임 시작 시 테이블 한번 생성
    /// </summary>
    public override void Start()
    {
        database.OpenDatabase();
        database.CreateTable();
    }
}
