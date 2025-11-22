using System.Collections.Generic;
using UnityEngine;
/*
public class BansheeAI : BossMonsterAI
{
    [Header("스킬 쿨타임 조정")]
    [SerializeField] private float screamCooldown = 10f;
    [SerializeField] private float teleportCooldown = 10f;
    [SerializeField] private float buffCooldown = 10f;
    [SerializeField] private float ultCooldown = 10f;
    
    // 이 변수들은 실제 쿨타임 값을 저장합니다.
    // 부모 클래스의 skillFirstCooldown, skillSecondCooldown 변수와 헷갈리지 않도록
    // MinoAI 클래스 내부에서만 사용하고, 이를 부모 클래스 변수에 할당하는 방식이 좋습니다.
    
    // Start() 메서드에서 부모 클래스의 쿨타임 변수에 값을 할당합니다.
    protected override void Start()
    {
        base.Start();
        // 부모 클래스의 변수에 값을 할당하여 공통 로직을 활용합니다.
        skillFirstCooldown = screamCooldown;
        skillSecondCooldown = teleportCooldown;
        skillThirdCooldown = buffCooldown;
        skillFourthCooldown = ultCooldown;
        
        // AttackCooldown은 부모 클래스에서 바로 사용 가능합니다.
    }

    // ---------------- BT patterns ----------------

    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        // 첫 번째 스킬 (Scream)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, model.AttackRange + 3, 0),
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 두 번째 스킬 (teleport)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, 100, model.AttackRange + 1),
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 세 번째 스킬 (Buff)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillThirdTimer)),
            new ActionNode(PerformSkillThird),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
        }));
        
        // 네 번째 스킬 (ultimate)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsSkillRangeNode(transform, player.transform, model.AttackRange, 0),
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
}*/