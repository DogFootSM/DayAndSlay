public class NewMonsterDieState : NewIMonsterState
{

    public void Enter(MonsterAnimator animator)
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
