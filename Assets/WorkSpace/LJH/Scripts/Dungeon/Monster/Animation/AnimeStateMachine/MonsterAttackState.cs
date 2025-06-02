using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
    {
        animator.PlayAttack();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
