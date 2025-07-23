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

            if (npc.CheckHeIsAngry())
            {
                Debug.Log("성문으로 이동");
                Vector3 castlePos = targetSensor.GetCastleDoorPosition();
                npc.StateMachine.ChangeState(new NpcMoveState(npc, castlePos));
            }

            else
            {
                Debug.Log("상점으로 이동");
                Vector3 storeDoorPos = targetSensor.GetEnterPosition();
                npc.StateMachine.ChangeState(new NpcMoveState(npc, storeDoorPos, new NpcIdleState(npc)));
            }
        }
        else
        {
            Debug.Log("랜덤 구역으로 이동");
            Vector3 randomPos = targetSensor.GetRandomPosition();

            npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos, new NpcIdleState(npc)));
        }
    }

    public void Update() { }
    public void Exit() { }
}