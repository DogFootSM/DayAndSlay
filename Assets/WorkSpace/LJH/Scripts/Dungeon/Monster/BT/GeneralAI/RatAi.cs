using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAi : GeneralMonsterAI
{
    protected override void Idle()
    {
        stateMachine.ChangeState(new MonsterIdleState());
    }
    protected override void Attack()
    {
        stateMachine.ChangeState(new MonsterAttackState());
    }

    protected override void Move()
    {
        stateMachine.ChangeState(new MonsterMoveState());
    }
}
