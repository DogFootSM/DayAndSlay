using UnityEngine;

/// <summary>
/// 상점 내부에서 행동을 결정하는 상태
/// </summary>
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
        
        if (npc.CheckHeIsAngry())
        {
            Debug.Log("구매자이고 화가난 경우에서 곤스테이트 호출");
            npc.StateMachine.ChangeState(new NpcMoveState(npc, targetSensor.GetLeavePosition() + new Vector3(0, -2f, 0), new NpcGoneState(npc)));
            store.MinusRepu(10);
            return;
        }
        // 랜덤 포지션으로 이동
        Vector3 randomPos = targetSensor.GetRandomPositionInStore();
        
        npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos,
            new NpcIdleState(npc))); 
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}