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

        if (table != null)
        {
            npc.SetTargetTable(table);
            npc.StateMachine.ChangeState(new MoveState(npc, table.transform.position));
        }
        else
        {
            npc.StateMachine.ChangeState(new NpcDecisionInStoreState(npc, npc.GetStoreManager(), npc.GetSensor()));
        }
    }

    public void Update() { }
    public void Exit() { }
}