public class MonsterMoveState : IMonsterState
{

    public void Enter(GeneralAnimator animator)
    {
        animator.PlayMove();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
