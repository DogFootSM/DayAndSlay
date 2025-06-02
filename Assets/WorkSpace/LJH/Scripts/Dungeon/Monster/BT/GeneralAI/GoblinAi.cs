using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAi : GeneralMonsterAI
{

    public override void Idle()
    {
        if (isAction) return;

        Debug.Log("고블린 대기");
        stateMachine.ChangeState(new MonsterIdleState());
    }
    public override void Attack()
    {
        if (isAction) return;

        Debug.Log("고블린 공격");
        stateMachine.ChangeState(new MonsterAttackState());
    }

    public override void Move()
    {
        if (isAction) return;

        Debug.Log("고블린 이동");
        stateMachine.ChangeState(new MonsterMoveState());
    }
}
