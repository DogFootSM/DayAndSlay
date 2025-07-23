using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLoggingState : INpcState
{
    private Npc npc;
    private float time;


    public NpcLoggingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        time = Random.Range(30f, 60f);
    }

    public void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            npc.SetMoving(false);
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
    }

    public void Exit()
    {

    }
}
