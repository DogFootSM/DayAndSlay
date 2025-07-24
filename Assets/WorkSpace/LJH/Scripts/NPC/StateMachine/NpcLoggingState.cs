using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹ú¸ñ »óÅÂ
/// </summary>
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
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
    }

    public void Exit()
    {

    }
}
