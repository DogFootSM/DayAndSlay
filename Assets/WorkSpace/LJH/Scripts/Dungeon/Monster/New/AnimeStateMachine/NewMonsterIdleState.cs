public class NewMonsterIdleState : NewIMonsterState
{

    public void Enter(NewMonsterAnimator animator)
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
