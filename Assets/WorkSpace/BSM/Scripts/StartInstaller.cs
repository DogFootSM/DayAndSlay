using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class StartInstaller : MonoInstaller
{
    public CanvasManager UICanvasManager;
    [Inject]
    SqlManager sqlManager;
    
    public override void InstallBindings()
    {
        Container.Bind<CanvasManager>().FromInstance(UICanvasManager);
    }

    /// <summary>
    /// 게임 시작 시 테이블 한번 생성
    /// </summary>
    public override void Start()
    {
        Debug.Log("sql 호출");
        sqlManager.ReadDataColumn<int>("slot_id");
    }
}
