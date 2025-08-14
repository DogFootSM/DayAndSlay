public class MonsterDieState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
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
