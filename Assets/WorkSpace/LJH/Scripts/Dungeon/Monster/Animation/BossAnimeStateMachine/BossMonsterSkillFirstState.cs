public class BossMonsterSkillFirstState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlaySkillFirst();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}