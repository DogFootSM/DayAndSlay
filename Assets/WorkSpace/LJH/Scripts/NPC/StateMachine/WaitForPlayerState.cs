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
        if (npc.GetStoreManager().PeekInNpcQue() == npc)
        {
            npc.WantItemMarkOnOff(Emoji.EXCLAMATION);
            npc.TestCoroutine();
        }
    }

    public void Update()
    {
    }
    public void Exit() { }
}
