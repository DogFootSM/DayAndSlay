public class LeaveState : INpcState
{
    private NpcNew npc;

    public LeaveState(NpcNew npc)
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