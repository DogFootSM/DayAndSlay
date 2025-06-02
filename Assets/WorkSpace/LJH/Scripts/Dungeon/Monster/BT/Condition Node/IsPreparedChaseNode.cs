using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IsPreparedChaseNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private float range;             // 기준 거리

    public IsPreparedChaseNode(Transform self, Transform target, float range)
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
