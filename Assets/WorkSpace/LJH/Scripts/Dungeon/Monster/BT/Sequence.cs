using System.Collections.Generic;

public class Sequence : BTNode
{
    List<BTNode> nodes = new List<BTNode>();

    public Sequence(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Tick()
    {
        foreach (BTNode node in nodes)
        {
            NodeState result = node.Tick();

            if (result == NodeState.Failure) return result;

            else if(result == NodeState.Running) return result;
        }

        return NodeState.Success;
    }
}
