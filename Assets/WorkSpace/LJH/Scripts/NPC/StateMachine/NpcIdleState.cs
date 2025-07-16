public class NpcIdleState : INpcState
{
    private NpcNew npc;

    public NpcIdleState(NpcNew npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.SetMoving(false);
        npc.SearchTable();
    }

    public void Update() { }
    public void Exit() { }
}