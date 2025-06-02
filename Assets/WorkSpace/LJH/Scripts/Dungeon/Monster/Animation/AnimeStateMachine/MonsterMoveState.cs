using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
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
