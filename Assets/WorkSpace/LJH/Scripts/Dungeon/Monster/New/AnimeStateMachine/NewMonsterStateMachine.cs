public class NewMonsterStateMachine
{
    public NewIMonsterState currentState;
    NewMonsterAnimator animator;

    public NewMonsterStateMachine(NewMonsterAnimator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(NewIMonsterState newState)
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
