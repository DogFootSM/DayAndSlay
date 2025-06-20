using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterIdleState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlayIdle();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
