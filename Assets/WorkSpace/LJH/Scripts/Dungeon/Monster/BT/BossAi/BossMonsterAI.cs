using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BossMonsterAI : MonoBehaviour
{
    [SerializeField] protected TestPlayer player;
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected BossAnimator animator;

    protected BehaviourTree tree;

    protected MonsterStateMachine stateMachine;
    protected MonsterModel model;
    protected GeneralMonsterMethod method;

    public M_State monsterState;

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        method = GetComponent<GeneralMonsterMethod>();
        if (animator == null)
            animator = GetComponent<BossAnimator>();
    }

    protected virtual void Start()
    {
        tree = new BehaviourTree(BuildRoot());
    }

    protected virtual void Update()
    {
        tree?.Tick();
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
        List<BTNode> skillSelector = BuildSkillSelector() ?? new List<BTNode>();
        
        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackCooldown),
            new Selector(skillSelector)
        };
    }

    protected virtual List<BTNode> BuildAttackSequence()
    {
        List<BTNode> attackSelector = BuildAttackSelector() ?? new List<BTNode>();
        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new Selector(attackSelector)
        };
    }

    protected List<BTNode> BuildIdleSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedIdleNode(transform, player.transform, monsterData.ChaseRange),
            new ActionNode(Idle)
        };
    }
    protected virtual List<BTNode> BuildDieSequence()
    {
        // 죽음 체크 시퀀스
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
        Debug.Log($"{name} -> Idle");
        animator.stateMachine.ChangeState(new BossMonsterIdleState());
        //animator.PlayIdle();
    }

    protected virtual void Move()
    {
        Debug.Log($"{name} -> Move");
    }

    protected virtual void Attack()
    {
        Debug.Log($"{name} -> Attack");
    }

    public virtual void Die()
    {
        Debug.Log($"{name} -> Die");
        // 실제 사망 로직 (애니메이션, 상태 전환 등)
        // method?.DieMethod();
        // stateMachine?.ChangeState(new MonsterDieState());
    }

    public MonsterData GetMonsterData()
    {
        return monsterData;
    }
}