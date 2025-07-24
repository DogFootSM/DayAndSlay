using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPreFishingState : INpcState
{
    private Npc npc;

    public NpcPreFishingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        npc.transform.position = npc.GetSensor().GetFishingRandomPosition();
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
