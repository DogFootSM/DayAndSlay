public class BossMonsterSkillSecondState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlaySkillSecond();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}