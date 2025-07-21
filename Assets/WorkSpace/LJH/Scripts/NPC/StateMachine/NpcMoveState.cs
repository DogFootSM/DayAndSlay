using UnityEngine;

public class NpcMoveState : INpcState
{
    private Npc npc;
    private Vector3 target;
    private INpcState nextState;

    public NpcMoveState(Npc npc, Vector3 target, INpcState nextState = null)
    {
        this.npc = npc;
        this.target = target;
        this.nextState = nextState ?? new NpcIdleState(npc); // 기본값
    }

    public void Enter()
    {
        npc.MoveTo(target, () =>
        {
            // 상태가 아직 MoveState일 때만 다음 상태로 전환
            if (npc.StateMachine.CurrentState == this)
            {
                npc.StateMachine.ChangeState(nextState);
            }
        });
    }

    public void Update()
    {
        if (npc.StateMachine.CurrentState != this)
            return;

        if (npc.ArrivedDesk())
        {
            npc.StateMachine.ChangeState(new WaitForPlayerState(npc));
        }
    }
    public void Exit() { }
}
