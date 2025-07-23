using UnityEngine;

public class NpcLeaveState : INpcState
{
    private Npc npc;
    private StoreManager storeManager;
    public NpcLeaveState(Npc npc)
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