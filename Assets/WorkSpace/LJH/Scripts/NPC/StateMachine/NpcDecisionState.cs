using UnityEngine;

/// <summary>
/// 구매 희망 여부에 따른 행동 선택
/// </summary>
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

        if (DayManager.instance.GetDayOrNight() == DayAndNight.NIGHT)
        {
            Vector3 castlePos = targetSensor.GetCastleDoorPosition();
            Debug.Log("디시션 엔터에서 곤스테이트 호출");
            npc.StateMachine.ChangeState(new NpcMoveState(npc, castlePos, new NpcGoneState(npc)));
            return;
        }

        if (npc.IsBuyer)
        {
            Vector3 storeDoorPos = targetSensor.GetEnterPosition();
            npc.StateMachine.ChangeState(new NpcMoveState(npc, storeDoorPos, new NpcIdleState(npc)));
        }
        else
        {
            Vector3 randomPos = targetSensor.GetRandomPosition();

            npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos, new NpcIdleState(npc)));
        }
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}