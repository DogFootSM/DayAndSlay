using System.Collections;
using UnityEngine;

/// <summary>
/// Npc 이동 이후 멈춤 상태 > 이후 현재 위치 및 특이사항에 따라 어디로 이동할지 다시 결정
/// </summary>
public class NpcIdleState : INpcState
{
    private Npc npc;
    private Grid grid;
    
    private Coroutine decideCo;

    public NpcIdleState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.StopMove();
        decideCo = npc.StartCoroutine(WaitAndDecide());
    }

    private IEnumerator WaitAndDecide()
    {
        yield return new WaitForSeconds(1f);
        //저녁일 경우 바로 성으로 돌아감
        
        ///Npc의 현재 위치가 Outside인 경우
        if (npc.IsInOutsideGrid())
        {
            Debug.Log("외부로 인식됨");
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
        
        ///Npc의 현재 위치가 상점 내부인 경우
        else
        {
            npc.StateMachine.ChangeState(new NpcSearchTableState(npc));
            

            if (npc.GetStoreManager().PeekInNpcQue() == npc)
            {
                //npc.StateMachine.ChangeState(new NpcMoveState(npc, npc.GetSensor().GetDeskPosition()));
            }

        }
    }

    public void Update() { }

    public void Exit()
    {
        npc.StopCoroutine(decideCo);
    }

}
