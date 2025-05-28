using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : BTNode
{
    private System.Action performIdle;

    public IdleNode(System.Action performIdle)
    {
        this.performIdle = performIdle;
    }

    public override NodeState Tick()
    {
        performIdle?.Invoke();
        return NodeState.Success;
    }
}
