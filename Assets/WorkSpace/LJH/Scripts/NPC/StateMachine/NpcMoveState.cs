using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoveState : INpcState
{
    private NPC npc;
    private Vector3 targetPos;
    private AstarPath astar;

    public NpcMoveState(NPC npc, Vector3 targetPos)
    {
        this.npc = npc;
        astar = npc.GetComponentInChildren<AstarPath>();
        this.targetPos = targetPos;
    }

    public void Enter()
    {
        Debug.Log($"지정된 대상의 위치 {targetPos}");
        npc.targetSensor.targetPos = this.targetPos;
        astar.DetectTarget(npc.transform.position, targetPos);
        npc.NPCMove();
    }

    public void Update()
    {
        if (Vector3.Distance(npc.transform.position, targetPos) <= 1f)
        {
            
        }
    }

    public void Exit()
    {
        npc.SetMoving(false);
    }
}
