using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(GeneralMonsterMethod))]
[RequireComponent(typeof(GeneralAnimator))]
[RequireComponent(typeof(MonsterModel))]
public class GeneralMonsterAI : MonoBehaviour
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected PlayerController player;

    protected BehaviourTree tree;
    protected MonsterModel model;
    protected GeneralMonsterMethod method;
    protected GeneralAnimator animator;

    protected MonsterStateMachine stateMachine;

    public M_State monsterState;
    public bool isAttacking = false;

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        method = GetComponent<GeneralMonsterMethod>();
        animator = GetComponent<GeneralAnimator>();
    }

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        tree = new BehaviourTree(BuildRoot());
        stateMachine = new MonsterStateMachine(animator);

        method.MonsterDataInit(monsterData);
    }

    protected virtual void Update()
    {
        tree.Tick();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(1); // 테스트용
        }
#endif
    }

    protected virtual BTNode BuildRoot()
    {
        return new Selector(new List<BTNode>
        {
            new Sequence(BuildDieSequence()),
            new Sequence(BuildAttackSequence()),
            new Sequence(BuildChaseSequence()),
            new Sequence(BuildIdleSequence())
        });
    }

    protected virtual List<BTNode> BuildDieSequence()
    {
        return new List<BTNode>
        {
            new IsDieNode(() => model.Hp),
            new ActionNode(Die)
        };
    }

    protected virtual List<BTNode> BuildAttackSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new ActionNode(Attack)
        };
    }

    protected virtual List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedChaseNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackRange),
            new ActionNode(Move)
        };
    }

    protected virtual List<BTNode> BuildIdleSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedIdleNode(transform, player.transform, monsterData.ChaseRange, () => !isAttacking),
            new ActionNode(Idle)
        };
    }

    // ========== 상태 메서드 ==========
    protected virtual void Idle()
    {
        if (monsterState == M_State.IDLE) return;

        stateMachine.ChangeState(new MonsterIdleState());
        monsterState = M_State.IDLE;
    }

    protected virtual void Move()
    {
        if (monsterState == M_State.MOVE || method.isMoving) return;

        stateMachine.ChangeState(new MonsterMoveState());
        method.MoveMethod();
        monsterState = M_State.MOVE;
    }

    protected virtual void Attack()
    {
        if (monsterState == M_State.ATTACK) return;

        stateMachine.ChangeState(new MonsterAttackState());
        method.StopMoveCo();
        method.isAttacking = true;
        isAttacking = true;
        monsterState = M_State.ATTACK;

        StartCoroutine(AttackEndDelay());
    }

    protected virtual void Die()
    {
        Debug.Log($"{name} → Die");
        method.DieMethod();
        stateMachine.ChangeState(new MonsterDieState());
    }

    public virtual void Hit(int damage)
    {
        method.HitMethod(damage);
        stateMachine.ChangeState(new MonsterHitState());
    }

    protected IEnumerator AttackEndDelay()
    {
        yield return new WaitForSeconds(0.5f); // 애니메이션 길이

        isAttacking = false;
        method.isAttacking = false;
        monsterState = M_State.IDLE;
    }

    public MonsterData GetMonsterData() => monsterData;
    public MonsterModel GetMonsterModel() => model;
}