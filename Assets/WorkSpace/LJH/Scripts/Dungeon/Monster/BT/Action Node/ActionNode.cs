public class ActionNode : BTNode
{
    private System.Action performAction;

    public ActionNode(System.Action performAction)
    {
        this.performAction = performAction;
    }

    public override NodeState Tick()
    {
        performAction?.Invoke();
        return NodeState.Success;
    }
}