using System.Collections.Generic;
using UnityEngine;

public class MinoAI : BossAI
{
    private bool isMinoGiga = false;

    public bool SetIsMinoGiga(bool isMinoGiga) => this.isMinoGiga = isMinoGiga;
    // ---------------- BT patterns ----------------

    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

       // 첫 번째 스킬 (Butt)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, firstSkillData),
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 두 번째 스킬 (stomp)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, secondSkillData),
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 세 번째 스킬 (Gigantism)
        list.Add(new Sequence(new List<BTNode>
        {
            new WaitWhileActionNode(() => isMinoGiga),
            new IsHPThresholdCheckNode(50f, GetMonsterModel()),
            new IsPreparedCooldownNode(() => CanSkill(skillThirdTimer)),
            new ActionNode(PerformSkillThird),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 네 번째 스킬 (ultimate)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsHPThresholdCheckNode(30f, GetMonsterModel()),
            new IsPreparedCooldownNode(() => CanSkill(skillFourthTimer)),
            new ActionNode(PerformSkillFourth),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));

        return list;
    }

    protected override List<BTNode> BuildAttackSelector()
    {
        List<BTNode> list = new List<BTNode>();

        // 일반 공격
        list.Add(new Sequence(new List<BTNode>
        {
            new IsAttackRangeNode(transform, player.transform, model.AttackRange),
            new IsPreparedCooldownNode(CanAttack),
            new ActionNode(PerformAttack),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));

        return list;
    }

}