using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
    {
        animator.PlayHit();
    }
    public void Update()
    {
    }
    public void Exit()
    {
    }
}
