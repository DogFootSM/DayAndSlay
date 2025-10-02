using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 벌목 준비 상태
/// </summary>
public class NpcPreLoggingState : INpcState
{
    private Npc npc;

    public NpcPreLoggingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        npc.StopMove();
        npc.StateMachine.ChangeState(new NpcLoggingState(npc));
    }

    public void Update()
    {
    }

    public void Exit()
    {

    }

}
