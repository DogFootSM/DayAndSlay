using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldNpcMoveState : INpcState
{
    private NPC npc;
    private Vector3 targetPos;
    private AstarPath astar;

    public OldNpcMoveState(NPC npc, Vector3 targetPos)
    {
        this.npc = npc;
        astar = npc.GetComponentInChildren<AstarPath>();
        this.targetPos = targetPos;
    }

    public void Enter()
    {
        npc.targetSensor.targetPos = this.targetPos;
        astar.DetectTarget(npc.transform.position, targetPos);
        npc.NPCMove();
    }

    public void Update()
    {
    }

    public void Exit()
    {
        npc.SetMoving(false);
    }
}
