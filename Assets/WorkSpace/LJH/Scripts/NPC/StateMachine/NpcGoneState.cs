using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Npc 비활성화
/// </summary>
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
