using UnityEngine;

public class NpcDecisionInStoreState : INpcState
{
    StoreManager store;
    private Npc npc;
    TargetSensorInNpc targetSensor;

    public NpcDecisionInStoreState(Npc npc, StoreManager store, TargetSensorInNpc targetSensor)
    {
        this.npc = npc;
        this.store = store;
        this.targetSensor = targetSensor;
    }

    public void Enter()
    {
        if (npc == store.PeekInNpcQue())
        {
            //카운터로 이동
            Vector3 deskPos = targetSensor.GetDeskPosition();
            npc.StateMachine.ChangeState(new NpcMoveState(npc, deskPos));

        }
        else
        {
            // 랜덤 포지션으로 이동
            Vector3 randomPos = targetSensor.GetRandomPositionInStore();
            npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos, new NpcDecisionInStoreState(npc, store, targetSensor)));
        }
    }

    public void Update() 
    {
    }
    public void Exit() { }
}
