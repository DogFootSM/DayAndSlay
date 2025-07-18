using System.Collections;
using UnityEngine;

public class NpcIdleState : INpcState
{
    private Npc npc;
    private Grid grid;

    public NpcIdleState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.SetMoving(false);
        npc.StartCoroutine(WaitAndDecide());
    }

    private IEnumerator WaitAndDecide()
    {
        grid = npc.GetComponentInChildren<TargetSensorInNpc>().GetCurrentGrid(npc.transform.position);

        yield return new WaitForSeconds(1f); // 또는 랜덤 시간
        if (grid.name == "OutsideGrid")
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        else
            npc.StateMachine.ChangeState(new NpcSearchTableState(npc));
    }

    public void Update() { }
    public void Exit() { }
}
