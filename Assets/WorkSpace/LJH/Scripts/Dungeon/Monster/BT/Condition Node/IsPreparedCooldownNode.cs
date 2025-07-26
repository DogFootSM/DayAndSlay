using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPreparedCooldownNode : BTNode
{
    private bool canAttack;         // 공격 쿨다운
    private readonly Func<bool> condition; // 조건 델리게이트

    public IsPreparedCooldownNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeState Tick()
    {
        if (condition != null && condition())
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
