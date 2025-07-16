public class NpcInteractPlayerState : INpcState
{
    private NpcNew npc;

    public NpcInteractPlayerState(NpcNew npc)
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