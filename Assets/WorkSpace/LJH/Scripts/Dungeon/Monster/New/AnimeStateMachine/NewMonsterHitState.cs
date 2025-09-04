public class NewMonsterHitState : NewIMonsterState
{

    public void Enter(NewMonsterAnimator animator)
    {
        animator.PlayHit();
    }
    public void Update()
    {
    }
    public void Exit()
    {
    }
}
