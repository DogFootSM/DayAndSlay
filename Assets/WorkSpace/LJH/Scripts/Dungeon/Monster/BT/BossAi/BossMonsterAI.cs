using System.Collections.Generic;
using UnityEngine;

// 모든 보스 몬스터의 공통 로직을 담는 추상 클래스
public abstract class BossMonsterAI : NewMonsterAI
{
    // 네펜데스AI에서 가져온 공통 쿨타임 변수들
    [Header("공통 스킬 쿨타임 조정")]
    [SerializeField] protected float skillFirstCooldown;
    [SerializeField] protected float skillSecondCooldown;
    [SerializeField] protected float attackCooldown = 2f;

    // 네펜데스AI에서 가져온 공통 타이머 변수들
    protected float skillFirstTimer;
    protected float skillSecondTimer;
    protected float attackTimer;

    protected override void Start()
    {
        base.Start();
        
        skillFirstTimer = skillFirstCooldown;
        skillSecondTimer = skillSecondCooldown;
        attackTimer = attackCooldown;
    }
    protected override void Update()
    {
        base.Update();
        UpdateCooldowns();
    }

    // 쿨타임 업데이트 로직도 모든 보스에게 공통
    protected void UpdateCooldowns()
    {
        skillFirstTimer -= Time.deltaTime;
        skillSecondTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }

    protected override BTNode BuildRoot()
    {
        return new Selector(new List<BTNode>
        {
            new Sequence(BuildDieSequence()),
            new Sequence(BuildSkillSequence()),
            new Sequence(BuildAttackSequence()),
            new Sequence(BuildChaseSequence()),
            new Sequence(BuildIdleSequence())
        });
    }

    protected virtual List<BTNode> BuildSkillSequence()
    {
        List<BTNode> patterns = BuildSkillSelector();
        if (patterns == null) patterns = new List<BTNode>();

        return new List<BTNode>
        {   
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange + 1, monsterData.AttackCooldown),
            new Selector(patterns)
        };
    }

    protected override List<BTNode> BuildAttackSequence()
    {
        List<BTNode> patterns = BuildAttackSelector();
        if (patterns == null) patterns = new List<BTNode>();

        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new Selector(patterns)
        };
    }

    
    // 이 메서드들은 자식 클래스에서 오버라이딩하여 구현
    protected abstract List<BTNode> BuildSkillSelector();
    protected abstract List<BTNode> BuildAttackSelector();

    // ============================ 동작 메서드 =============================
    
    protected abstract bool IsAllOnCooldown();
    protected abstract bool CanSkillFirst();
    protected abstract bool CanSkillSecond();
    protected abstract bool CanAttack();

    protected void ResetSkillFirstCooldown() => skillFirstTimer = skillFirstCooldown;
    protected void ResetSkillSecondCooldown() => skillSecondTimer = skillSecondCooldown;
    protected void ResetAttackCooldown() => attackTimer = attackCooldown;

    protected void PerformSkillFirst()
    {
        stateMachine.ChangeState(new NewMonsterSkillFirstState(transform, player.transform));
        isAttacking = true;
        method.Skill_First();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillFirstCooldown();
    }
    
    protected void PerformSkillSecond()
    {
        stateMachine.ChangeState(new NewMonsterSkillSecondState(transform, player.transform));
        isAttacking = true;
        method.Skill_Second();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillSecondCooldown();
    }

    protected void PerformAttack()
    {
        stateMachine.ChangeState(new NewMonsterAttackState(transform, player.transform));
        isAttacking = true;
        method.AttackMethod();
        StartCoroutine(AttackEndDelay());
        ResetAttackCooldown();
    }

    protected void EndAction()
    {
        AttackCommonEnd();
    }

    // 이하 기존 코드 유지
    protected virtual void AttackCommonStart()
    {
        
    }

    protected virtual void SkillCommonStart()
    {
        
    }

    protected virtual void AttackCommonEnd()
    {
        
    }
}