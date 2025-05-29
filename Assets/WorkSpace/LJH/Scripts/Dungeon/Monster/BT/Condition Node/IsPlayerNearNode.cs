using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerNearNode : BTNode
{
    NodeState state;

    float playerDistance;
    float attackDistance;
    bool isPlayerNear = false;
    public override NodeState Tick()
    {
        if (isPlayerNear) return state = NodeState.Success;

        else return state = NodeState.Failure;
    }
}
