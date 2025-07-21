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
        var targetSensor = npc.GetComponentInChildren<TargetSensorInNpc>();

        if (npc.IsBuyer)
        {
            Debug.Log("상점으로 이동");
            Vector3 storeDoorPos = targetSensor.GetEnterPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, storeDoorPos, new NpcIdleState(npc)));
        }
        else
        {
            Debug.Log("랜덤 포지션으로 이동");
            Vector3 randomPos = targetSensor.GetRandomPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, randomPos, new NpcIdleState(npc)));
        }
    }

    public void Update() { }
    public void Exit() { }
}