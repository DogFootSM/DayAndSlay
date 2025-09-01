using System.Collections.Generic;
using UnityEngine;

// 모든 보스 몬스터의 공통 로직을 담는 추상 클래스
public abstract class BossMonsterAI : NewMonsterAI
{
    // 네펜데스AI에서 가져온 공통 쿨타임 변수들
    [Header("공통 스킬 쿨타임 조정")]
    protected float skillFirstCooldown;
    protected float skillSecondCooldown;
    protected float skillThirdCooldown;
    protected float skillFourthCooldown;
    
    [SerializeField] protected float attackCooldown = 2f;

    // 네펜데스AI에서 가져온 공통 타이머 변수들
    protected float skillFirstTimer;
    protected float skillSecondTimer;
    protected float skillThirdTimer;
    protected float skillFourthTimer;
    
    protected float attackTimer;

    protected override void Start()
    {
        base.Start();
        
        skillFirstTimer = skillFirstCooldown;
        skillSecondTimer = skillSecondCooldown;
        skillThirdTimer = skillThirdCooldown;
        skillFourthTimer = skillFourthCooldown;
        
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
        skillThirdTimer -= Time.deltaTime;
        skillFourthTimer -= Time.deltaTime;
        
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

    //이걸로 스킬 시퀀스 만들어줌
    protected virtual List<BTNode> BuildSkillSequence()
    {
        List<BTNode> patterns = BuildSkillSelector();
        if (patterns == null) patterns = new List<BTNode>();

        return new List<BTNode>
        {   
            new IsPreparedAttackNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackCooldown),
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


    protected bool CanSkill(float timer)
    {
        return timer <= 0f;
    }
    protected bool CanAttack()
    {
        // 일반 공격 쿨타임을 체크합니다.
        return attackTimer <= 0f;
    }

    protected void ResetSkillFirstCooldown() => skillFirstTimer = skillFirstCooldown;
    protected void ResetSkillSecondCooldown() => skillSecondTimer = skillSecondCooldown;
    protected void ResetSkillThirdCooldown() => skillThirdTimer = skillThirdCooldown;
    protected void ResetSkillFourthCooldown() => skillFourthTimer = skillFourthCooldown;
    protected void ResetAttackCooldown() => attackTimer = attackCooldown;

    protected void PerformSkillFirst()
    {
        Debug.Log("스킬 1번 사용");
        stateMachine.ChangeState(new NewMonsterSkillFirstState(transform, player.transform));
        isAttacking = true;
        method.Skill_First();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillFirstCooldown();
    }
    
    protected void PerformSkillSecond()
    {
        Debug.Log($"{gameObject.name}스킬 2번 사용");
        stateMachine.ChangeState(new NewMonsterSkillSecondState(transform, player.transform));
        isAttacking = true;
        method.Skill_Second();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillSecondCooldown();
    }
    
    protected void PerformSkillThird()
    {
        Debug.Log("스킬 3번 사용");
        stateMachine.ChangeState(new NewMonsterSkillThirdState(transform, player.transform));
        isAttacking = true;
        method.Skill_Third();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillThirdCooldown();
    }
    
    protected void PerformSkillFourth()
    {
        stateMachine.ChangeState(new NewMonsterSkillFourthState(transform, player.transform));
        isAttacking = true;
        method.Skill_Fourth();
        SkillCommonStart();
        StartCoroutine(AttackEndDelay());
        ResetSkillFourthCooldown();
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