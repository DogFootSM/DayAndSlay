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

        if (npc.IsBuyer)
        {
            npc.SearchTable();
        }
        else
        {
            npc.StateMachine.ChangeState(new NpcDecisionState(npc));
        }
    }

    public void Update() { }
    public void Exit() { }
}
