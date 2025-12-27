using UnityEngine;

public class IsPreparedChaseNode : BTNode
{
    private Transform self;        // 몬스터
    private Transform target;      // 플레이어
    private MonsterModel model;    // 몬스터 모델 (사거리 참조용)

    public IsPreparedChaseNode( Transform self, Transform target, MonsterModel model)
    {
        this.self = self;
        this.target = target;
        this.model = model;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);

        // 추격 사거리 안 + 공격 사거리 밖
        if (distance <= model.ChaseRange && distance > model.AttackRange)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
