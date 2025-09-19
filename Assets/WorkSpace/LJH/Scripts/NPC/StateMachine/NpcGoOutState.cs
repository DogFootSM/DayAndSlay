using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGoOutState : INpcState
{
    private Npc npc;
    
    public NpcGoOutState(Npc npc)
    {
        this.npc = npc;
    }
    
    public void Enter()
    {
        npc.GoHome();
    }

    public void Update()
    {
    }

    public void Exit()
    {
        npc.SetIsNightTrue();
    }
}
