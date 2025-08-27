using UnityEngine;

public class IsHPThresholdCheckNode : BTNode
{
    private float threshold;
    private MonsterModel model;
    
    /// <summary>
    /// 체력이 일정 이하로 떨어졌을 시 스킬 체크
    /// </summary>
    /// <param name="threshold"></param>
    /// <param name="curHp"></param>
    public IsHPThresholdCheckNode(float threshold,  MonsterModel model)
    {
        this.threshold = threshold;
         this.model = model;
    }

    public override NodeState Tick()
    {
        float hpPer = ((float)model.Hp / model.MaxHp) * 100;
        Debug.Log($"현재 보스몬스터의 체력 비율{hpPer}");
        if (threshold >= hpPer)
        {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}