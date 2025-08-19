public class NewMonsterAttackState : NewIMonsterState
{

    public void Enter(NewMonsterAnimator animator)
    {
        animator.PlayAttack();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
