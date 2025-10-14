using UnityEngine;

/// <summary>
/// NPC 이동 상태
/// </summary>
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
        //초기화
        npc.StopMove();
        npc.UpdateGrid();
        
        Debug.Log($"목표 위치는 {target}");
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


        if (npc.ArrivedDesk() && npc.GetStoreManager().PeekInNpcQue() == npc)
        {
            npc.StateMachine.ChangeState(new WaitForPlayerState(npc));
        }
    }

    public void Exit()
    {
        npc.StopMove();
    }
}
