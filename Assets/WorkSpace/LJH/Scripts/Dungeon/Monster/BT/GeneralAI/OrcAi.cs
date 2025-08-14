public class OrcAi : GeneralMonsterAI
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
