using UnityEngine;

public class IsPreparedSkillNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private MonsterModel model;      //  모델

    /// <summary>
    /// 
    /// </summary>
    /// <param name="self">자신</param>
    /// <param name="target">플레이어</param>
    /// <param name="range">거리</param>
    public IsPreparedSkillNode(Transform self, Transform target, MonsterModel model)
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