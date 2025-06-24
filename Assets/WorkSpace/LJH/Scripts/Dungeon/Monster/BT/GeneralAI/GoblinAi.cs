using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAi : GeneralMonsterAI
{
    // AI가 움직여야겠다 라고 판단했을 때 무브를 실행해
    // AI가 공격해야겠다 라고 판단했을 때 어택을 실행해
    // AI가 할게 없을 때 아이들을 실행해

    //Idle이 아닐때에만 Idle을 실행해야지
    //움직이는 중이 아닐 때에만 Move를 실행하고
    // 공격중이 아닐때에만 Attack을 실행하고


    public override void Idle()
    {
        Debug.Log("몬스터 대기 실행");
        stateMachine.ChangeState(new MonsterIdleState());
        monsterState = M_State.IDLE;
    }
    public override void Attack()
    {
        Debug.Log("공격 함수 실행");
        if (monsterState != M_State.ATTACK)
        {
            Debug.Log("몬스터 공격 실행");
            stateMachine.ChangeState(new MonsterAttackState());

            monsterState = M_State.ATTACK;
            method.StopMoveCo();
            method.isAttacking = true;
        }
    }

    public override void Move()
    {
        Debug.Log("이동 함수 실행");
        if (!method.isMoving && monsterState == M_State.IDLE)
        {
            Debug.Log("몬스터 이동 실행");
            stateMachine.ChangeState(new MonsterMoveState());
            monsterState = M_State.MOVE;
            method.MoveMethod();
        }
    }
}
