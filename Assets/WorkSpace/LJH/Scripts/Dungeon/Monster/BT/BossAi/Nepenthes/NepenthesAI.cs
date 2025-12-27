using System.Collections.Generic;
using UnityEngine;

// BossMonsterAI를 상속받고, 네펜데스 보스만의 고유한 로직을 구현합니다.
public abstract class NepenthesAI : BossAI
{
    [Header("파트너")]
    [SerializeField] protected BossAI partner;

    protected override void Start()
    {
        base.Start();
        
        skillFirstTimer = firstSkillData.CoolDown;
        skillSecondTimer = secondSkillData.CoolDown;
        skillThirdTimer = thirdSkillData.CoolDown;
        skillFourthTimer = 0;
        
        attackTimer = model.AttackCooldown;
    }
    // 네펜데스 보스의 고유한 특성: 움직이지 않음
    protected override List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new AlwaysFailNode()
        };
    }
}