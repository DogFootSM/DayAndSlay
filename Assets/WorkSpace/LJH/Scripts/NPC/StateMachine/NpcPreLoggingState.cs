using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPreLoggingState : INpcState
{
    private Npc npc;

    public NpcPreLoggingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        npc.transform.position = npc.GetSensor().GetLoggingRandomPosition();
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
