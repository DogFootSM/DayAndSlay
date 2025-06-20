using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackRangeNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private float range;             // 기준 거리

    public IsAttackRangeNode(Transform self, Transform target, float range)
    {
        this.self = self;
        this.target = target;
        this.range = range;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);
        if (distance <= range)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
