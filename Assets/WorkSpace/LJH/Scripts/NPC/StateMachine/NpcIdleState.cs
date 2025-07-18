using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : INpcState
{
    private Npc npc;
    private GameObject target;

    public NpcIdleState(Npc npc, GameObject target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        npc.SetMoving(false);

        if (npc.GetComponentInChildren<TargetSensorInNpc>().GetCurrentGrid(npc.gameObject.transform.position).name == "OutsideGrid")
        {
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
        else
        {
            npc.SearchTable();
        }

    }

    public void Update() { }
    public void Exit() { }
}
