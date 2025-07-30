using System.Collections.Generic;
using UnityEngine;

public abstract class BossMonsterAI : MonoBehaviour
{
    [SerializeField] protected TestPlayer player;
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected BossAnimator animator;
    [SerializeField] protected BossMonsterMethod method;

    protected BehaviourTree tree;

    protected MonsterModel model;
    
    public MonsterModel GetMonsterModel() => model;

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        if (animator == null) animator = GetComponent<BossAnimator>();
        if (method == null) method = GetComponent<BossMonsterMethod>();
    }

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<TestPlayer>();
        tree = new BehaviourTree(BuildRoot());
        method.MonsterDataInit(monsterData);
        method.PlayerInit(player.gameObject);
    }

    protected virtual void Update()
    {
        if (tree != null)
        {
            tree.Tick();
        }
    }

    protected virtual BTNode BuildRoot()
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

    /// <summary>
    /// BT 추격 시퀀스 생성
    /// </summary>
    /// <returns></returns>
    protected virtual List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedChaseNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackRange),
            new ActionNode(Move)
        };
    }

    /// <summary>
    /// BT 스킬 시퀀스 생성
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// BT 공격 시퀀스 생성
    /// </summary>
    /// <returns></returns>
    protected virtual List<BTNode> BuildAttackSequence()
    {
        List<BTNode> patterns = BuildAttackSelector();
        if (patterns == null) patterns = new List<BTNode>();

        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new Selector(patterns)
        };
    }

    /// <summary>
    /// BT 대기 시퀀스 생성
    /// </summary>
    /// <returns></returns>
    protected virtual List<BTNode> BuildIdleSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedIdleNode(transform, player.transform, monsterData.ChaseRange, IsAllOnCooldown),
            new ActionNode(Idle)
        };
    }

    /// <summary>
    /// BT 사망 시퀀스 생성
    /// </summary>
    /// <returns></returns>
    protected virtual List<BTNode> BuildDieSequence()
    {
        return new List<BTNode>
        {
            new IsDieNode(() => model.Hp),
            new ActionNode(Die)
        };
    }

    protected abstract List<BTNode> BuildSkillSelector();
    protected abstract List<BTNode> BuildAttackSelector();

    // ============================ 동작 메서드 =============================
    
    /// <summary>
    /// 각 몬스터별 클래스에서 쿨타임 체크 넣어줘야 함
    /// </summary>
    /// <returns></returns>
    protected abstract bool IsAllOnCooldown();
    
    protected virtual void Idle()
    {
        Debug.Log(name + " -> Idle");
        animator.stateMachine.ChangeState(new BossMonsterIdleState());
    }

    protected virtual void Move()
    {
        Debug.Log(name + " -> Move");
        animator.stateMachine.ChangeState(new BossMonsterMoveState());
        method.Move();
    }

    protected virtual void AttackCommonStart()
    {
        animator.stateMachine.ChangeState(new BossMonsterAttackState());
        method.BeforeAttack();
        method.Attack();
    }

    protected virtual void SkillCommonStart()
    {
        animator.stateMachine.ChangeState(new BossMonsterSkillState());
        method.BeforeAttack();
    }

    //엔드의 경우 스킬과 공격 공통
    protected virtual void AttackCommonEnd()
    {
        method.AfterAttack();
    }

    

    protected virtual void Die()
    {
        Debug.Log(name + " -> Die");
        animator.PlayDie();
        method.Die();
    }

    public MonsterData GetMonsterData()
    {
        return monsterData;
    }
}
