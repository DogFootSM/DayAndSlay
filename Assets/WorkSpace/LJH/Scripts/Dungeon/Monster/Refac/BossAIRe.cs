using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 보스 몬스터의 공통 로직을 담는 추상 클래스
public abstract class BossAIRe : MonsterAI
{
    [SerializeField] protected MonsterSkillData firstSkillData;
    [SerializeField] protected MonsterSkillData secondSkillData;
    [SerializeField] protected MonsterSkillData thirdSkillData;
    [SerializeField] protected MonsterSkillData fourthSkillData;

    public float skillFirstTimer;
    public float skillSecondTimer;
    public float skillThirdTimer;
    public float skillFourthTimer;
    
    public float attackTimer;

    protected override void Start()
    {
        base.Start();
        
        skillFirstTimer = firstSkillData.CoolDown;
        skillSecondTimer = secondSkillData.CoolDown;
        skillThirdTimer = thirdSkillData.CoolDown;
        skillFourthTimer = fourthSkillData.CoolDown;
        
        attackTimer = model.AttackCooldown;
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
            new Sequence(BuildStunSequence()),
            new Sequence(BuildSkillSequence()),
            //new Sequence(BuildAttackSequence()),
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

    protected void ResetSkillFirstCooldown() => skillFirstTimer = firstSkillData.CoolDown;
    protected void ResetSkillSecondCooldown() => skillSecondTimer = secondSkillData.CoolDown;
    protected void ResetSkillThirdCooldown() => skillThirdTimer = thirdSkillData.CoolDown;
    protected void ResetSkillFourthCooldown() => skillFourthTimer = fourthSkillData.CoolDown;
    protected void ResetAttackCooldown() => attackTimer = model.AttackCooldown;

    protected void PerformSkillFirst()
    {
        stateMachine.ChangeState(new NewMonsterSkillFirstState(transform, player.transform));
        isSkillUsing = true;
        method.Skill_First();
        StartCoroutine(SkillEndCoroutine());
        ResetSkillFirstCooldown();
    }
    
    protected void PerformSkillSecond()
    {
        stateMachine.ChangeState(new NewMonsterSkillSecondState(transform, player.transform));
        isSkillUsing = true;
        method.Skill_Second();
        StartCoroutine(SkillEndCoroutine());
        ResetSkillSecondCooldown();
    }
    
    protected void PerformSkillThird()
    {
        stateMachine.ChangeState(new NewMonsterSkillThirdState(transform, player.transform));
        isSkillUsing = true;
        method.Skill_Third();
        StartCoroutine(SkillEndCoroutine());
        ResetSkillThirdCooldown();
    }
    
    protected void PerformSkillFourth()
    {
        stateMachine.ChangeState(new NewMonsterSkillFourthState(transform, player.transform));
        isSkillUsing = true;
        method.Skill_Fourth();
        StartCoroutine(SkillEndCoroutine());
        ResetSkillFourthCooldown();
    }

    protected void PerformAttack()
    {
        stateMachine.ChangeState(new NewMonsterAttackState(transform, player.transform));
        isAttacking = true;
        StartCoroutine(AttackEndDelay());
        ResetAttackCooldown();
    }

    protected IEnumerator SkillEndCoroutine()
    {
        yield return new WaitUntil(() =>!animator.IsPlayingAction);
        isSkillUsing = false;
    }


   
}