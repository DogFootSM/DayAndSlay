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

        //현재 이 위치에서 벗어나지 않으니 낚시 포인트와 이 포인트를 반복하느라 안움직임
        if(Vector3.Distance(npc.transform.position, npc.GetSensor().GetFishingPosition()) <= 1f)
        {
            Debug.Log("낚시");
            npc.StateMachine.ChangeState(new NpcPreFishingState(npc));
        }

        if(Vector3.Distance(npc.transform.position, npc.GetSensor().GetLoggingPosition()) <= 1f)
        {
            Debug.Log("벌목");
            npc.StateMachine.ChangeState(new NpcPreLoggingState(npc));
        }
    }
    public void Exit() { }
}
