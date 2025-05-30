using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
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
