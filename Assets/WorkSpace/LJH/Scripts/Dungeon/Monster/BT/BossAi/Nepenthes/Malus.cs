using System.Collections.Generic;
using UnityEngine;

public class Malus : NepenthesAI
{
    [Header("소환 조건 조정")]
    [SerializeField] private float summonThresholdPercent = 20f;
    
    protected override void Start()
    {
        base.Start();
        // 파트너 찾기 로직
        partner = FindObjectOfType<Bellus>();
    }
    protected override bool IsAllOnCooldown()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > monsterData.ChaseRange)
        {
            return true;
        }

        if (distance > monsterData.AttackRange)
        {
            return !CanSkillSecond() && !CanSkillFirst();
        }

        return !CanSkillSecond() && !CanSkillFirst() && !CanAttack();
    }

    // 스킬 첫 번째 (소환) 사용 조건
    protected override bool CanSkillFirst()
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

    // 스킬 두 번째 (뿌리 공격) 사용 조건
    protected override bool CanSkillSecond()
    {
        return skillSecondTimer <= 0f;
    }

    // 일반 공격 사용 조건
    protected override bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    // 스킬 행동 트리 패턴 구현
    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();
        
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillFirst),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));

        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillSecond),
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