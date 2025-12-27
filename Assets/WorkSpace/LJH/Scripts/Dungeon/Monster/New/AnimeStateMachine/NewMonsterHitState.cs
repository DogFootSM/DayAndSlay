public class NewMonsterHitState : NewIMonsterState
{

    public void Enter(MonsterAnimator animator)
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
