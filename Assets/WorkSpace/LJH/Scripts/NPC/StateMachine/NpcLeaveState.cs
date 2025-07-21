public class LeaveState : INpcState
{
    private Npc npc;
    private StoreManager storeManager;
    public LeaveState(Npc npc)
    {
        this.npc = npc;
        storeManager = npc.GetStoreManager();
    }

    public void Enter()
    {
        storeManager.DequeueInNpcQue();
        npc.LeaveStore();
    }

    public void Update() { }
    public void Exit() { }
}