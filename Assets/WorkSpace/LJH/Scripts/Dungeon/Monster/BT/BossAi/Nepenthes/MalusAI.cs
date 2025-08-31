using System.Collections.Generic;
using UnityEngine;

public class MalusAI : NepenthesAI
{
    [Header("소환 조건 조정")]
    [SerializeField] private float summonThresholdPercent = 20f;

    [SerializeField] private float rootAttackCooldown = 10f;
    [SerializeField] private float summonCooldown = 10f;
    [SerializeField] private float cooldown = 10f;
    
    protected override void Start()
    {
        base.Start();
        // 파트너 찾기 로직
        partner = FindObjectOfType<BellusAI>();

        skillFirstCooldown = rootAttackCooldown;
        skillSecondCooldown = summonCooldown;
        skillThirdCooldown = cooldown;
    }

    // 스킬 첫 번째 (소환) 사용 조건
    protected bool CanSkillCondition()
    {
        if (partner == null) return false;
        if (skillFirstTimer > 0f) return false;

        MonsterModel myModel = model;
        MonsterModel partnerModel = partner.GetComponent<MonsterModel>();
        if (myModel == null || partnerModel == null) return false;

        float myHpPercent = (float)myModel.GetMonsterHp() / (float)myModel.GetMonsterMaxHp() * 100f;
        float partnerHpPercent = (float)partnerModel.GetMonsterHp() / (float)partnerModel.GetMonsterMaxHp() * 100f;
        float diff = Mathf.Abs(myHpPercent - partnerHpPercent);

        return diff >= summonThresholdPercent;
    }


    // 스킬 행동 트리 패턴 구현
    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();
        
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillCondition),
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }

    // 공격 행동 트리 패턴 구현
    protected override List<BTNode> BuildAttackSelector()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsAttackRangeNode(transform, player.transform, 2f),
            new IsPreparedCooldownNode(CanAttack),
            new ActionNode(PerformAttack),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        return list;
    }
}