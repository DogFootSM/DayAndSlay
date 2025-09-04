using System;
using UnityEngine;

public class WaitWhileActionNode : BTNode
{
    private Func<bool> isPlayingAction;

    public WaitWhileActionNode(Func<bool> isPlayingAction)
    {
        this.isPlayingAction = isPlayingAction;
    }

    public override NodeState Tick()
    {
        if (isPlayingAction())
        {
            return NodeState.Running;
        }

        return NodeState.Success;
    }
}