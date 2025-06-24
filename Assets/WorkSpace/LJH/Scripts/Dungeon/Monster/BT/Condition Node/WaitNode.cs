using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : BTNode
{
    private Func<bool> conditionCheck;

    public WaitNode(Func<bool> conditionCheck)
    {
        this.conditionCheck = conditionCheck;
    }

    public override NodeState Tick()
    {
        if (conditionCheck())
            return NodeState.Success;
        else
            return NodeState.Running;
    }
}
