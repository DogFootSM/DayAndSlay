using UnityEngine;

public class IsPreparedAttackNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private float range;             // 기준 거리
    private float cooldown;         // 공격 쿨다운
    private float currentCooldown; // 남은 쿨다운

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self">자신</param>
    /// <param name="target">플레이어</param>
    /// <param name="range">거리</param>
    public IsPreparedAttackNode(Transform self, Transform target, float range, float cooldown)
    {
        this.self = self;
        this.target = target;
        this.range = range;
        this.cooldown = cooldown;
        currentCooldown = 0;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);

        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;


        if (distance <= range && currentCooldown <= 0f)
        {
            currentCooldown = cooldown;
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
