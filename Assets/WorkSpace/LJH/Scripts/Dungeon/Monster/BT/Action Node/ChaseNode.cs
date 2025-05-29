using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : BTNode
{
    private System.Action performChase;

    public ChaseNode(System.Action performChase)
    {
        this.performChase = performChase;
    }

    public override NodeState Tick()
    {
        performChase?.Invoke();
        return NodeState.Success;
    }

}
