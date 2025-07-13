using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoveState : INpcState
{
    private NPC npc;
    private Vector3 targetPos;

    public NpcMoveState(NPC npc, GameObject targetPos)
    {
        this.npc = npc;
        this.targetPos = targetPos.transform.position;
    }

    public void Enter()
    {
        npc.targetSensor.targetPos = this.targetPos;
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
