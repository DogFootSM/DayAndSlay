public class LeaveState : INpcState
{
    private Npc npc;

    public LeaveState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.LeaveStore();
    }

    public void Update() { }
    public void Exit() { }
}