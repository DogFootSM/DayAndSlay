using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAi : GeneralMonsterAI
{

    public override void Idle()
    {
        Debug.Log("몬스터 대기 실행");
        stateMachine.ChangeState(new MonsterIdleState());
    }
    public override void Attack()
    {
        Debug.Log("몬스터 공격 실행");
        stateMachine.ChangeState(new MonsterAttackState());
    }

    public override void Move()
    {
        Debug.Log("몬스터 이동 실행");
        stateMachine.ChangeState(new MonsterMoveState());
        method.Move();
    }
}
