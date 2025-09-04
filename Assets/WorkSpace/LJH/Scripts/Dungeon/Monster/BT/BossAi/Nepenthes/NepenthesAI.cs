using System.Collections.Generic;
using UnityEngine;

// BossMonsterAI를 상속받고, 네펜데스 보스만의 고유한 로직을 구현합니다.
public abstract class NepenthesAI : BossMonsterAI
{
    [Header("파트너")]
    [SerializeField] protected BossMonsterAI partner;

    // 네펜데스 보스의 고유한 특성: 움직이지 않음
    protected override List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new AlwaysFailNode()
        };
    }

    // 네펜데스 보스의 고유한 특성: 파트너를 가짐
    public NepenthesAI GetPartner() => partner.GetComponent<NepenthesAI>();
}