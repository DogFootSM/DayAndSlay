public class BossMonsterAttackState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
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
