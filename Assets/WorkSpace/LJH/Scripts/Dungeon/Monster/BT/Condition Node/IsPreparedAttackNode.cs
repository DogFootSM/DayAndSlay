using UnityEngine;

public class IsPreparedAttackNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private MonsterModel model;      //  모델
    private float currentCooldown; // 남은 쿨다운

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self">자신</param>
    /// <param name="target">플레이어</param>
    /// <param name="range">거리</param>
    public IsPreparedAttackNode(Transform self, Transform target, MonsterModel model)
    {
        this.self = self;
        this.target = target;
        this.model = model;
        currentCooldown = 0;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);

        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;


        if (distance <= model.AttackRange && currentCooldown <= 0f)
        {
            currentCooldown = model.AttackCooldown;
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
