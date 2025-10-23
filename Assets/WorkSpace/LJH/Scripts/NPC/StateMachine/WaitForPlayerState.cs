using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 거래를 수락하길 기다리는 상태
/// </summary>
public class WaitForPlayerState : INpcState
{
    private Npc npc;
    public WaitForPlayerState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.StopMove();
        npc.RigidbodyZero();  
        
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
