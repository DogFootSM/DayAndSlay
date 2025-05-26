using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseNode : BTNode
{
    NodeState state;

    public override NodeState Tick()
    {
        return state;
    }

}
