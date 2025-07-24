using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ³¬½Ã »óÅÂ
/// </summary>
public class NpcFishingState : INpcState
{
    private Npc npc;
    private float time;

    public NpcFishingState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        time = Random.Range(3f, 6f);

    }

    public void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            npc.SetMoving(false);
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
    }

    public void Exit()
    {

    }

}
