public class AlwaysFailNode : BTNode
{
    public override NodeState Tick()
    {
        return NodeState.Failure;
    }
}