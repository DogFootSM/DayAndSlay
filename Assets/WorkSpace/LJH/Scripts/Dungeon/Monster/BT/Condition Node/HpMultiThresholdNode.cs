using System.Collections.Generic;
using UnityEngine;

public class HpMultiThresholdNode : BTNode
{
    private MonsterModel model;
    private List<float> thresholds;         // 예: 80, 60, 40, 20
    private HashSet<float> triggeredSet;    // 발동된 threshold 저장

    public HpMultiThresholdNode(MonsterModel model, List<float> thresholds)
    {
        this.model = model;
        this.thresholds = thresholds;
        triggeredSet = new HashSet<float>();
    }

    public override NodeState Tick()
    {
        float hpPer = ((float)model.CurHp / model.MaxHp) * 100f;

        // 아직 발동되지 않은 threshold 중에서 HP가 이하가 된 첫 번째 값 찾기
        foreach (float th in thresholds)
        {
            if (!triggeredSet.Contains(th) && hpPer <= th)
            {
                triggeredSet.Add(th); // 발동 처리
                return NodeState.Success; // BT에서 트리거 발생
            }
        }

        return NodeState.Failure;
    }
}