using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayerState : INpcState
{
    private Npc npc;
    public WaitForPlayerState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        
    }

    public void Update() 
    {
        if (Vector3.Distance(npc.transform.position, npc.GetSensor().GetDeskPosition()) <= 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                npc.TalkToPlayer();
            }
        }
    }
    public void Exit() { }
}
