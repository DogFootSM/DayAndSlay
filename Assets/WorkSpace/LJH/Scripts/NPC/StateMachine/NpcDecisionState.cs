using UnityEngine;

public class NpcDecisionState : INpcState
{
    private NpcNew npc;

    public NpcDecisionState(NpcNew npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        if (npc.IsBuyer)
        {
            //상점 문 위치로 이동
            Vector3 storeDoorPos = npc.GetComponentInChildren<TargetSensorNew>().GetEnterPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, storeDoorPos));
        }
        else
        {
            // 랜덤 포지션으로 이동
            Vector3 randomPos = npc.GetComponentInChildren<TargetSensorNew>().GetRandomPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, randomPos));
        }
    }

    public void Update() { }
    public void Exit() { }
}