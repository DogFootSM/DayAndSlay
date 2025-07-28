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

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        if (animator == null) animator = GetComponent<BossAnimator>();
        if (method == null) method = GetComponent<BossMonsterMethod>();
    }

    protected virtual void Start()
    {
        tree = new BehaviourTree(BuildRoot());
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

    protected virtual List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedChaseNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackRange),
            new ActionNode(Move)
        };
    }

    protected virtual List<BTNode> BuildSkillSequence()
    {
        List<BTNode> patterns = BuildSkillSelector();
        if (patterns == null) patterns = new List<BTNode>();

        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new Selector(patterns)
        };
    }

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

    protected virtual List<BTNode> BuildIdleSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedIdleNode(transform, player.transform, monsterData.ChaseRange),
            new ActionNode(Idle)
        };
    }

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

    protected virtual void Idle()
    {
        Debug.Log(name + " -> Idle");
        animator.PlayIdle();
    }

    protected virtual void Move()
    {
        Debug.Log(name + " -> Move");
        animator.PlayMove();
        method.Move();
    }

    protected virtual void AttackCommonStart()
    {
        animator.PlayAttack();
        method.BeforeAttack();
    }

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
