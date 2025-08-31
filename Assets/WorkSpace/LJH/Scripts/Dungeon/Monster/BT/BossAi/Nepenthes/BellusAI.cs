using System.Collections.Generic;
using UnityEngine;

public class BellusAI : NepenthesAI
{
    [Header("힐 조건 조정")]
    [SerializeField] private float healThresholdPercent = 20f;
    
    [SerializeField] private float poisonCooldown = 10f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float cooldown = 10f;
    
    protected override void Start()
    {
        base.Start();
        // 파트너 찾기 로직
        partner = FindObjectOfType<MalusAI>();
        
        skillFirstCooldown = poisonCooldown;
        skillSecondCooldown = healCooldown;
        skillThirdCooldown = cooldown;
    }

    // 스킬 첫 번째 (힐) 사용 조건


    private bool CanSkillCondition()
    {
        if (partner == null) return false;
        if (skillFirstTimer > 0f) return false;

        MonsterModel myModel = model;
        MonsterModel partnerModel = partner.GetComponent<MonsterModel>();
        if (myModel == null || partnerModel == null) return false;

        float myHpPercent = (float)myModel.GetMonsterHp() / (float)myModel.GetMonsterMaxHp() * 100f;
        float partnerHpPercent = (float)partnerModel.GetMonsterHp() / (float)partnerModel.GetMonsterMaxHp() * 100f;
        float diff = Mathf.Abs(myHpPercent - partnerHpPercent);

        return diff >= healThresholdPercent;
    }

    // 스킬 행동 트리 패턴 구현
    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillFirstCooldown)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillSecondCooldown)),
            new IsPreparedCooldownNode(CanSkillCondition),
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