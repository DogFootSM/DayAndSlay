public class NpcIdleState : INpcState
{
    private Npc npc;

    public NpcIdleState(Npc npc)
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
