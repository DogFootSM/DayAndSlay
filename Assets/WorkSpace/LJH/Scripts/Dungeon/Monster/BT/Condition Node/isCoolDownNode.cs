using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class IsCooldownNode : BTNode
{
    private Transform self;    // 몬스터
    private Transform target;  // 플레이어
    private float range;       // 기준 거리

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self">자신</param>
    /// <param name="target">플레이어</param>
    /// <param name="range">거리</param>
    public IsCooldownNode(Transform self, Transform target, float range)
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
