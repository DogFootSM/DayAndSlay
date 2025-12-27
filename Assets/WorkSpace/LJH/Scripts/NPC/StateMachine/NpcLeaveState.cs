using UnityEngine;

/// <summary>
/// 상점을 떠나는 상태
/// </summary>
public class NpcLeaveState : INpcState
{
    private Npc npc;
    public NpcLeaveState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.StopMove();
        npc.LeaveStore();
    }

    public void Update() { }
    public void Exit() { }
}