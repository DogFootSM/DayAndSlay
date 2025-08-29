using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonsterAI : MonoBehaviour
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected PlayerController player;

    protected BehaviourTree tree;
    protected MonsterModel model;
    protected NewMonsterMethod method;
    protected NewMonsterAnimator animator;

    protected NewMonsterStateMachine stateMachine;

    public bool isAttacking = false;

    private Vector3 lastPlayerPos;

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        method = GetComponent<NewMonsterMethod>();
        animator = GetComponent<NewMonsterAnimator>();
        lastPlayerPos = transform.position;
    }

    protected virtual void Start()
    {
        StartCoroutine(StartDelayCoroutine());
    }

    private IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        
        player = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        method.SetPlayer(player.gameObject);
        
        tree = new BehaviourTree(BuildRoot());

        stateMachine = new NewMonsterStateMachine(animator);

        method.MonsterDataInit(monsterData);
    }

    protected virtual void Update()
    {
        if (tree == null) return;
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
            new ActionNode(Idle)
        };
    }

    // ========== 상태 메서드 ==========
    protected void Idle()
    {
        stateMachine.ChangeState(new NewMonsterIdleState());
        method.IdleMethod();
    }

    protected void Move()
    {
        stateMachine.ChangeState(new NewMonsterMoveState(transform, player.transform));
        
        //플레이어가 움직인 경우
        if (PlayerIsMoved() == true)
        {
            // 몬스터의 현재 위치와 플레이어의 현재 위치를 인자로 넘겨줍니다.
            method.RequestPathUpdate(transform.position, player.transform.position);
        }

        method.MoveMethod();
        lastPlayerPos = player.transform.position;
    }

    private bool PlayerIsMoved()
    {
        return Vector3.Distance(lastPlayerPos, player.transform.position) > 0.01f;
    }

    protected virtual void Attack()
    {
        stateMachine.ChangeState(new NewMonsterAttackState(transform, player.transform));
        isAttacking = true;
        method.AttackMethod();
        StartCoroutine(AttackEndDelay());
    }

    protected virtual void Die()
    {
        method.DieMethod();
        stateMachine.ChangeState(new NewMonsterDieState());
    }

    public virtual void Hit(int damage)
    {
        method.HitMethod(damage);
        stateMachine.ChangeState(new NewMonsterHitState());
    }

    protected IEnumerator AttackEndDelay()
    {
        yield return new WaitForSeconds(model.AttackCooldown);
    
        isAttacking = false;
    }

    public MonsterData GetMonsterData() => monsterData;
    public MonsterModel GetMonsterModel() => model;
    public NewMonsterMethod GetMonsterMethod() => method;
    public NewMonsterAnimator GetMonsterAnimator() => animator;
    
    public float GetChaseRange() => monsterData.ChaseRange;
}
