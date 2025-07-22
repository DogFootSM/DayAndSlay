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
        npc.WantItemMarkOnOff();
        npc.TestCoroutine();
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //플레이어에서 호출하는 것으로 구조 변경 필요
        //    npc.TalkToPlayer();
        //    npc.StateMachine.ChangeState(new NpcWaitItemState(npc));
        //}
    }
    public void Exit() { }
}
