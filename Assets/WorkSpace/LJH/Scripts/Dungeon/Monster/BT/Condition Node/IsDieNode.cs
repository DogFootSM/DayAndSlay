using System;

public class IsDieNode : BTNode
{
    private Func<float> getHp;

    public IsDieNode(Func<float> getHp)
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
