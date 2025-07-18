using UnityEngine;

public class MoveState : INpcState
{
    private Npc npc;
    private Vector3 target;
    private INpcState nextState;

    public MoveState(Npc npc, Vector3 target, INpcState nextState = null)
    {
        this.npc = npc;
        this.target = target;
        this.nextState = nextState ?? new NpcIdleState(npc); // ±âº»°ª
    }

    public void Enter()
    {
        npc.MoveTo(target, () =>
        {
            npc.StateMachine.ChangeState(nextState);
        });
    }

    public void Update() { }
    public void Exit() { }
}
