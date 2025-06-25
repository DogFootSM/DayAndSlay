using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IsPreparedChaseNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private float chaseRange;             // 기준 거리
    private float attackRange;      // 몬스터 사거리

    public IsPreparedChaseNode(Transform self, Transform target, float chaseRange, float attackRange)
    {
        this.self = self;
        this.target = target;
        this.chaseRange = chaseRange;
        this.attackRange = attackRange;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
