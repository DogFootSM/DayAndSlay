public class BossMonsterStateMachine
{
    public IBossMonsterState currentState;
    BossAnimator animator;

    public BossMonsterStateMachine(BossAnimator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(IBossMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(animator);
    }

    public void Update()
    {
        currentState?.Update();
    }
}
