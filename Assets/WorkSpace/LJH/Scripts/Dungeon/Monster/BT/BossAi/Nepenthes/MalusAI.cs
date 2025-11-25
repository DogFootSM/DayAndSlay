using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalusAI : NepenthesAI
{
    [Header("소환 조건 조정")]
    [SerializeField] private float summonThresholdPercent = 0;


    // 스킬 첫 번째 (소환) 사용 조건
    protected bool CanSkillCondition()
    {
        if (partner == null) return false;
        
        MonsterModel myModel = model;
        MonsterModel partnerModel = partner.GetComponent<MonsterModel>();
        
        if (myModel == null || partnerModel == null) return false;
        
        float myHpPercent = (float)myModel.GetMonsterHp() / (float)myModel.GetMonsterMaxHp() * 100f;
        float partnerHpPercent = (float)partnerModel.GetMonsterHp() / (float)partnerModel.GetMonsterMaxHp() * 100f;
        float diff = Mathf.Abs(myHpPercent - partnerHpPercent);

        //벨루스의 체력이 나보다 낮을 경우 스킬 사용 조건 충족
        bool hpDiff = myHpPercent - partnerHpPercent >  0;
        
        return hpDiff;
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
            //new IsPreparedCooldownNode(CanSkillCondition),
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


    /// <summary>
    /// 스텟 변경하는 스킬이라 AI 클래스에서 호출
    /// </summary>
    public void Frenzy()
    {
        GetMonsterModel().AttackCooldown /= 2;
        //공속 쿨감 스킬임 메서드에서 해주자
    }
}
