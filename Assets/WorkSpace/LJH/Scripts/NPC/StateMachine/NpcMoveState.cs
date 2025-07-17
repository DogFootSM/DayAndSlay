using UnityEngine;

public class MoveState : INpcState
{
    private NpcNew npc;
    private Vector3 target;

    public MoveState(NpcNew npc, Vector3 target)
    {
        this.npc = npc;
        this.target = target;
    }

    public void Enter()
    {
        npc.MoveTo(target);
    }

    public void Update() { }
    public void Exit() { }
}