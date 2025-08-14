using UnityEngine;

public class GoblinAi : GeneralMonsterAI
{
    // AI가 움직여야겠다 라고 판단했을 때 무브를 실행해
    // AI가 공격해야겠다 라고 판단했을 때 어택을 실행해
    // AI가 할게 없을 때 아이들을 실행해

    //Idle이 아닐때에만 Idle을 실행해야지
    //움직이는 중이 아닐 때에만 Move를 실행하고
    // 공격중이 아닐때에만 Attack을 실행하고


    protected override void Idle()
    {
        if (monsterState == M_State.IDLE)
        {
            return;
        }

        stateMachine.ChangeState(new MonsterIdleState());
        monsterState = M_State.IDLE;
    }
    protected override void Attack()
    {
        if (monsterState == M_State.ATTACK)
        {
            return;
        }

        monsterState = M_State.ATTACK;
        stateMachine.ChangeState(new MonsterAttackState());
        method.StopMoveCo();
        method.isAttacking = true;

        StartCoroutine(AttackEndDelay()); // 공격 종료 타이밍 처리
    }

    protected override void Move()
    {
        if (!method.isMoving)
        {
            Debug.Log("AI에서 움직임 실행시킴");
            stateMachine.ChangeState(new MonsterMoveState());
            monsterState = M_State.MOVE;
            method.MoveMethod();
        }
    }
}
