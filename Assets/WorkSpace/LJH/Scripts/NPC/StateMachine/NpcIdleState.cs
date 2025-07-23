using System.Collections;
using UnityEngine;

public class NpcIdleState : INpcState
{
    private Npc npc;
    private Grid grid;

    public NpcIdleState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.SetMoving(false);
        npc.StartCoroutine(WaitAndDecide());
    }

    private IEnumerator WaitAndDecide()
    {
        //1등은 카운터로 나머지만 가게 뺑뺑이 로직 추가해야함

        //템 사고 나온놈은 캐슬도어로 가서 사라져야함

        yield return new WaitForSeconds(1f);
        if (npc.IsInOutsideGrid())
        {
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
        else
        {
            if(npc.GetStoreManager().PeekInNpcQue() == npc)
            {
                npc.StateMachine.ChangeState(new NpcMoveState(npc, npc.GetSensor().GetDeskPosition()));
            }

            npc.StateMachine.ChangeState(new NpcSearchTableState(npc));
        }
    }

    public void Update() { }
    public void Exit() { }
}
