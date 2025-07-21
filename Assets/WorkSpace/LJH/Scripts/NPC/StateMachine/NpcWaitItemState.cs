using UnityEngine;

public class NpcWaitItemState : INpcState
{
    private Npc npc;
    private float waitTime = 10f;
    private float elapsed = 0f;

    public NpcWaitItemState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        elapsed = 0f;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= waitTime)
        {
            npc.StateMachine.ChangeState(new NpcLeaveState(npc));
        }
    }

    public void Exit() { }
}