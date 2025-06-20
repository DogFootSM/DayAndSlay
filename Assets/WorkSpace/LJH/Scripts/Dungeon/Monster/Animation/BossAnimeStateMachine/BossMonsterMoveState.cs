using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterMoveState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlayMove();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
