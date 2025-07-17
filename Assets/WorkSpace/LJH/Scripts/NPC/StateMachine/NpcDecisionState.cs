using UnityEngine;

public class NpcDecisionState : INpcState
{
    private Npc npc;

    public NpcDecisionState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        if (npc.IsBuyer)
        {
            //상점 문 위치로 이동
            Vector3 storeDoorPos = npc.GetComponentInChildren<TargetSensorInNpc>().GetEnterPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, storeDoorPos));
        }
        else
        {
            // 랜덤 포지션으로 이동
            Vector3 randomPos = npc.GetComponentInChildren<TargetSensorInNpc>().GetRandomPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, randomPos));
        }
    }

    public void Update() { }
    public void Exit() { }
}