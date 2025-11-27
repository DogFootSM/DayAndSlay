using System.Collections;
using UnityEngine;

/// <summary>
/// NPC가 테이블을 탐색하여 원하는 아이템이 테이블에 있는지 확인하는 상태
/// </summary>
public class NpcSearchTableState : INpcState
{
    private Npc npc;

    public NpcSearchTableState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        Table table = npc.SearchTable();
        
        if (!npc.isSearchTableEnteredFirst)
        {
            npc.wantItemManager.ActiveWantItem(npc);
            npc.isSearchTableEnteredFirst = true;
        }

        if (table != null)
        {
            npc.SetTargetTable(table);
            npc.StateMachine.ChangeState(new NpcMoveState(npc, table.transform.position + new Vector3(0, -1, 0), new NpcItemBuyState(npc, table)));
        }
        else
        {
            npc.StateMachine.ChangeState(new NpcDecisionInStoreState(npc, npc.GetStoreManager(), npc.GetSensor()));
        }
    }


    public void Update() { }
    public void Exit() { }
}