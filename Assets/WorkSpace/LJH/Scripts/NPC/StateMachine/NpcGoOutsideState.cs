using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcGoOutsideState : INpcState
{
    private NPC npc;

    public NpcGoOutsideState(NPC npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        // 이동 완료 체크는 targetSensor에서 도착 이벤트를 던지게 할 수도
    }

    public void Exit()
    {
        // 필요 시 클린업
    }
}
