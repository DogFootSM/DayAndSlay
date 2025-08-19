public class NewMonsterMoveState : NewIMonsterState
{

    public void Enter(NewMonsterAnimator animator)
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
