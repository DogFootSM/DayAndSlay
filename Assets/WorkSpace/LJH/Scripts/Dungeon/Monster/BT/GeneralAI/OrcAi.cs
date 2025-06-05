using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAi : GeneralMonsterAI
{

    public override void Idle()
    {
        stateMachine.ChangeState(new MonsterIdleState());
    }
    public override void Attack()
    {
        stateMachine.ChangeState(new MonsterAttackState());
    }

    public override void Move()
    {
        stateMachine.ChangeState(new MonsterMoveState());
    }
}
