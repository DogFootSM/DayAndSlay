public class BossMonsterIdleState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlayIdle();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
