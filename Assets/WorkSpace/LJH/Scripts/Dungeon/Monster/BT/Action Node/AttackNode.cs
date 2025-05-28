using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : BTNode
{
    private System.Action performAttack;

    public AttackNode(System.Action performAttack)
    {
        this.performAttack = performAttack;
    }

    public override NodeState Tick()
    {
        performAttack?.Invoke();
        return NodeState.Success;
    }
}
