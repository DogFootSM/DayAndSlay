public class NewMonsterDieState : NewIMonsterState
{

    public void Enter(NewMonsterAnimator animator)
    {
        animator.PlayDie();
    }
    public void Update()
    {
    }
    public void Exit()
    {
    }
}
