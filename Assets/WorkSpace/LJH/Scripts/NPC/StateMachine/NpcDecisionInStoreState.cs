using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NpcDecisionInStoreState : INpcState
{
    StoreManager store;
    private Npc npc;
    TargetSensorInNpc targetSensor;

    public NpcDecisionInStoreState(Npc npc, StoreManager store)
    {
        this.npc = npc;
        this.store = store;
        targetSensor = npc.GetComponentInChildren<TargetSensorInNpc>();
    }

    public void Enter()
    {
        store.EnqueueInNpcQue(npc);
        if (npc.name == store.PeekInNpcQue().gameObject.name)
        {
            //카운터로 이동
            Vector3 deskPos = targetSensor.GetDeskPosition();
            npc.StateMachine.ChangeState(new MoveState(npc, deskPos));
        }
        else
        {
            // 랜덤 포지션으로 이동
            Vector3 randomPos = targetSensor.GetRandomPositionInStore();
            npc.StateMachine.ChangeState(new MoveState(npc, randomPos));
        }
    }

    public void Update() { }
    public void Exit() { }
}
