using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 낚시 준비 상태
/// </summary>
public class NpcPreFishingState : INpcState
{
    private Npc npc;

    public NpcPreFishingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        npc.StopMove();
        npc.StateMachine.ChangeState(new NpcFishingState(npc));
    }

    public void Update()
    {
    }

    public void Exit()
    {

    }

}
