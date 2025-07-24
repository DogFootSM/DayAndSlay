using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGoneState : INpcState
{
    private Npc npc;

    public NpcGoneState(Npc npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        npc.NpcGone();
    }

    public void Update()
    {
    }

    public void Exit()
    {

    }

}
