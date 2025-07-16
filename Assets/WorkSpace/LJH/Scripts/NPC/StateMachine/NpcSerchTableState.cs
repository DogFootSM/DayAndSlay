public class NpcSearchTableState : INpcState
{
    private NpcNew npc;

    public NpcSearchTableState(NpcNew npc)
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