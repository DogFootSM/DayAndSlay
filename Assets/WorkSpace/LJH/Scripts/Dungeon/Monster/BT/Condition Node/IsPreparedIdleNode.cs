using System;
using UnityEngine;

public class IsPreparedIdleNode : BTNode
{
    private Transform self;      // 몬스터
    private Transform target;    // 플레이어
    private float range;         // 기준 거리
    private Func<bool> cooldown;  // 추가 조건 (예: 모든 스킬 쿨타임)

    /// <summary>
    /// 체이스 범위를 벗어나거나, 모든 스킬이 쿨타임일 때 Idle 상태를 반환
    /// </summary>
    /// <param name="self">자신</param>
    /// <param name="target">플레이어</param>
    /// <param name="range">거리</param>
    /// <param name="cooldown">추가 조건</param>
    public IsPreparedIdleNode(Transform self, Transform target, float range, Func<bool> cooldown = null)
    {
        this.self = self;
        this.target = target;
        this.range = range;
        this.cooldown = cooldown;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);

        if (distance > range)
        {
            return NodeState.Success;
        }

        if (cooldown != null && cooldown())
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}