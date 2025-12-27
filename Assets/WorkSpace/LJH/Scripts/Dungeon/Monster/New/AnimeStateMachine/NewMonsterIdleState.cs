public class NewMonsterIdleState : NewIMonsterState
{

    public void Enter(MonsterAnimator animator)
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
