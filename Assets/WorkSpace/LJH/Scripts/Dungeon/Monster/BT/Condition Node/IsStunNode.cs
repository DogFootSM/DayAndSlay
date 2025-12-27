using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsStunNode : BTNode
{
    private bool isStun;

    public IsStunNode(bool isStun)
    {
        this.isStun = isStun;
    }

    public override NodeState Tick()
    {
        if (isStun)
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
