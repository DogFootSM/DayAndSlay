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
            Debug.Log("밤이라 집으로 돌아감");
            Vector3 castlePos = targetSensor.GetCastleDoorPosition();
            npc.StateMachine.ChangeState(new NpcMoveState(npc, castlePos, new NpcGoneState(npc)));
            return;
        }

        if (npc.IsBuyer)
        {
            Debug.Log($"{npc.name} 이게 터졉림");
            if (npc.CheckHeIsAngry())
            {
                Vector3 castlePos = targetSensor.GetCastleDoorPosition();
                npc.StateMachine.ChangeState(new NpcMoveState(npc, castlePos, new NpcGoneState(npc)));
            }

            else
            {
                Vector3 storeDoorPos = targetSensor.GetEnterPosition();
                npc.StateMachine.ChangeState(new NpcMoveState(npc, storeDoorPos, new NpcIdleState(npc)));
            }
        }
        else
        {
            Vector3 randomPos = targetSensor.GetRandomPosition();

            npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos, new NpcIdleState(npc)));
        }
    }

    public void Update() { }
    public void Exit() { }
}