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
    
    // IsAllOnCooldown은 미노의 스킬에 맞게 구현합니다.
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
    
    // MinoAI 고유의 스킬 사용 가능 조건만 정의합니다.
    protected override bool CanSkillFirst()
    {
        // Butt 공격 쿨타임을 체크합니다.
        return skillFirstTimer <= 0f;
    }

    protected override bool CanSkillSecond()
    {
        // Stomp 공격 쿨타임을 체크합니다.
        return skillSecondTimer <= 0f;
    }

    protected override bool CanAttack()
    {
        // 일반 공격 쿨타임을 체크합니다.
        return attackTimer <= 0f;
    }

    // ---------------- BT patterns ----------------

    protected override List<BTNode> BuildSkillSelector()
    {
        List<BTNode> list = new List<BTNode>();

        // 첫 번째 스킬 (Butt)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillFirst),
            new ActionNode(PerformSkillFirst),
            new WaitWhileActionNode(() => animator.IsPlayingAction),
            new ActionNode(EndAction)
        }));
        
        // 두 번째 스킬 (Stomp)
        list.Add(new Sequence(new List<BTNode>
        {
            new IsPreparedCooldownNode(CanSkillSecond),
            new ActionNode(PerformSkillSecond),
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