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
        this.nextState = nextState ?? new NpcIdleState(npc); // 기본값
    }

    public void Enter()
    {
        npc.MoveTo(target, () =>
        {
            npc.StateMachine.ChangeState(nextState);
        });
    }

    public void Update() 
    {
        if(npc.ArrivedDesk())
        {
            Debug.Log("물건 구매 상태로 전환");
            //npc.StateMachine.ChangeState(new BuyState(item));
            npc.StateMachine.ChangeState(new WaitForPlayerState(npc));
        }
    }
    public void Exit() { }
}
