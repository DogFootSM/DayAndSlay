using UnityEngine;

public class IsAttackRangeNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private MonsterModel model;             // 기준 거리

    public IsAttackRangeNode(Transform self, Transform target, MonsterModel model)
    {
        this.self = self;
        this.target = target;
        this.model = model;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);
        if (distance <= model.AttackRange)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
