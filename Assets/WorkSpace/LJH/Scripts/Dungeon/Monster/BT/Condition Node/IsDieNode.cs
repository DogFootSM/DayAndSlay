using System;

public class IsDieNode : BTNode
{
    private Func<int> getHp;

    public IsDieNode(Func<int> getHp)
    {
        this.getHp = getHp;
    }

    public override NodeState Tick()
    {
        if (getHp() <= 0)
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
