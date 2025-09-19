using System.Collections;
using UnityEngine;

/// <summary>
/// Npc 이동 이후 멈춤 상태 > 이후 현재 위치 및 특이사항에 따라 어디로 이동할지 다시 결정
/// </summary>
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
        npc.StartCoroutine(WaitAndDecide());
    }

    private IEnumerator WaitAndDecide()
    {
        //1등은 카운터로 나머지만 가게 뺑뺑이 로직 추가해야함

        //템 사고 나온놈은 캐슬도어로 가서 사라져야함

        yield return new WaitForSeconds(1f);
        //저녁일 경우 바로 성으로 돌아감
        
        if (npc.IsInOutsideGrid())
        {
            
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
        
        else
        {
            npc.StateMachine.ChangeState(new NpcSearchTableState(npc));

            if (npc.GetStoreManager().PeekInNpcQue() == npc)
            {
                npc.StateMachine.ChangeState(new NpcMoveState(npc, npc.GetSensor().GetDeskPosition()));
            }

        }
    }

    public void Update() { }
    public void Exit() { }

}
