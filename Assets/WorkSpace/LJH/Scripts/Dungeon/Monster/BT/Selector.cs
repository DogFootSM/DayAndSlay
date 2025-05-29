using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTNode
{
    List<BTNode> nodes = new List<BTNode>();

    public Selector(List<BTNode> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Tick()
    {
        foreach (BTNode node in nodes)
        {
            if (node == null) Debug.LogWarning("노드가 없음");
            NodeState result = node.Tick();

            if(result == NodeState.Success) return result;

            else if(result == NodeState.Running) return result;
        }

        return NodeState.Failure;
    }
}
