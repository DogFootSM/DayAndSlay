using UnityEngine;

/// <summary>
/// 상점을 떠나는 상태
/// </summary>
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