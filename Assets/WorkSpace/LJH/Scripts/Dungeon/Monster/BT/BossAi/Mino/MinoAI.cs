using System.Collections.Generic;
using UnityEngine;

public class MinoAI : BossMonsterAI
{
    // 미노타우로스만의 고유한 쿨타임 변수를 인스펙터에 노출합니다.
    // 기존 buttCooldown과 stompCooldown을 대체합니다.
    [Header("스킬 쿨타임 조정")]
    [SerializeField] private float buttCooldown = 10f;
    [SerializeField] private float stompCooldown = 10f;
    
    // 이 변수들은 실제 쿨타임 값을 저장합니다.
    // 부모 클래스의 skillFirstCooldown, skillSecondCooldown 변수와 헷갈리지 않도록
    // MinoAI 클래스 내부에서만 사용하고, 이를 부모 클래스 변수에 할당하는 방식이 좋습니다.
    
    // Start() 메서드에서 부모 클래스의 쿨타임 변수에 값을 할당합니다.
    protected override void Start()
    {
        base.Start();
        // 부모 클래스의 변수에 값을 할당하여 공통 로직을 활용합니다.
        skillFirstCooldown = buttCooldown;
        skillSecondCooldown = stompCooldown;
        
        // AttackCooldown은 부모 클래스에서 바로 사용 가능합니다.
    }

    // ---------------- BT patterns ----------------

    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        // 첫 번째 스킬 (Teleport)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillFirstTimer)),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));
        
        // 두 번째 스킬 (Scream)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillSecondTimer)),
            new ActionNode(PerformSkillSecond),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));
        
        // 세 번째 스킬 (Buff)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillThirdTimer)),
            new ActionNode(PerformSkillThird),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));
        
        // 네 번째 스킬 (ultimate)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(() => CanSkill(skillFourthTimer)),
            new ActionNode(PerformSkillFourth),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
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
            new ActionNode(EndAction)
        }));

        return list;
    }
}