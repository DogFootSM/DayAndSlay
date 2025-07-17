public class NpcInteractPlayerState : INpcState
{
    private Npc npc;

    public NpcInteractPlayerState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.TalkToPlayer();
    }

    public void Update() { }
    public void Exit() { }
}