public class NpcSearchTableState : INpcState
{
    private Npc npc;

    public NpcSearchTableState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.SearchTable();
    }

    public void Update() { }
    public void Exit() { }
}