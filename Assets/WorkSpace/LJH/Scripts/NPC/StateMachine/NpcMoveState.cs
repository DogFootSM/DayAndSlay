using UnityEngine;

public class MoveState : INpcState
{
    private Npc npc;
    private Vector3 target;

    public MoveState(Npc npc, Vector3 target)
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