using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellusAI : NepenthesAI
{
    [Header("힐 조건 조정")]
    [SerializeField] private float healThresholdPercent = 20f;
    
    [SerializeField] private float poisonCooldown = 10f;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float seedCooldown = 10f;
    
    protected override void Start()
    {
        base.Start();
        
        //skillFirstCooldown = poisonCooldown;
        //skillSecondCooldown = healCooldown;
        //skillThirdCooldown = seedCooldown;
    }


    // 스킬 첫 번째 (힐) 사용 조건


    private bool CanSkillCondition()
    {
        if (partner == null) return false;

        MonsterModel myModel = model;
        MonsterModel partnerModel = partner.GetComponent<MonsterModel>();
        if (myModel == null || partnerModel == null) return false;

        float myHpPercent = myModel.GetMonsterHp() / myModel.GetMonsterMaxHp() * 100f;
        float partnerHpPercent = partnerModel.GetMonsterHp() / partnerModel.GetMonsterMaxHp() * 100f;
        float diff = Mathf.Abs(myHpPercent - partnerHpPercent);
        Debug.Log($"체력 차 퍼센트 {diff}");
        return diff >= healThresholdPercent;
    }

    // 스킬 행동 트리 패턴 구현
    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillCondition),
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        list.Add(new Sequence(new List<BTNode>
        {
            new IsHPThresholdCheckNode(50f, model),
            new IsPreparedCooldownNode(() => CanSkill(skillThirdTimer)),
            new ActionNode(PerformSkillThird),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
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
        }));

        return list;
    }

}