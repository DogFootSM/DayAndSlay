public class SetAnimatorStateNode : BTNode
{
    private BossMonsterStateMachine stateMachine;
    private IBossMonsterState targetState;

    public SetAnimatorStateNode(BossMonsterStateMachine stateMachine, IBossMonsterState state)
    {
        this.stateMachine = stateMachine;
        this.targetState = state;
    }

    public override NodeState Tick()
    {
        stateMachine.ChangeState(targetState);
        return NodeState.Success;
}
    }
