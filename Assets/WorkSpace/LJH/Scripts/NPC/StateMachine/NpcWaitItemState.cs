using UnityEngine;

public class WaitItemState : INpcState
{
    private Npc npc;
    private float waitTime = 10f;
    private float elapsed = 0f;

    public WaitItemState(Npc npc)
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
            npc.StateMachine.ChangeState(new LeaveState(npc));
        }
    }

    public void Exit() { }
}