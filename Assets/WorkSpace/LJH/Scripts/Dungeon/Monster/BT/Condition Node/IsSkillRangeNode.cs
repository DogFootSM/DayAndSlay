using UnityEngine;

public class IsSkillRangeNode : BTNode
{
    private Transform self;            // 몬스터
    private Transform target;         // 플레이어
    private float maxRange;           // 최대 사거리
    private float minRange;           // 최소 사거리

    //public IsSkillRangeNode(Transform self, Transform target, float maxRange, float minRange)
    //{
    //    this.self = self;
    //    this.target = target;
    //    this.maxRange = maxRange;
    //    this.minRange = minRange;
    //}
    
    public IsSkillRangeNode(Transform self, Transform target, MonsterSkillData skillData)
    {
        this.self = self;
        this.target = target;
        maxRange = skillData.SkillMaxRange;
        minRange = skillData.SkillMinRange;
    }

    public override NodeState Tick()
    {
        float distance = Vector3.Distance(self.position, target.position);
        if (distance <= maxRange && distance >= minRange)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}