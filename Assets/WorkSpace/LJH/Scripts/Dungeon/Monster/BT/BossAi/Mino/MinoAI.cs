using System.Collections.Generic;
using UnityEngine;

public class MinoAI : BossMonsterAI
{
    [Header("스킬 쿨타임 조정")]
    [SerializeField] private float buttCooldown = 10f;
    [SerializeField] private float stompCooldown = 10f;
    [SerializeField] private float buffCooldown = 10f;
    [SerializeField] private float ultCooldown = 10f;
    
    
    // Start() 메서드에서 부모 클래스의 쿨타임 변수에 값을 할당
    protected override void Start()
    {
        base.Start();
        
        skillFirstCooldown = buttCooldown;
        skillSecondCooldown = stompCooldown;
        skillThirdCooldown = buffCooldown;
        skillFourthCooldown = ultCooldown;
    }

    // ---------------- BT patterns ----------------

    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

       // 첫 번째 스킬 (Butt)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, model.AttackRange +3, 0),
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 두 번째 스킬 (stomp)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, model.AttackRange +3, 0),
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 세 번째 스킬 (Buff)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, model.ChaseRange, model.AttackRange + 2),
            new IsHPThresholdCheckNode(80f, GetMonsterModel()),
            new IsPreparedCooldownNode(() => CanSkill(skillThirdTimer)),
            new ActionNode(PerformSkillThird),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 네 번째 스킬 (ultimate)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsHPThresholdCheckNode(50f, GetMonsterModel()),
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