public class BossMonsterSkillState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        animator.PlaySkill();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}