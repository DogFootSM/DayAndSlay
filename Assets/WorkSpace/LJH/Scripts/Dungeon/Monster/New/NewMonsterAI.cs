using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonsterAI : MonoBehaviour, IEffectReceiver
{
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected PlayerController player;

    protected BehaviourTree tree;
    protected MonsterModel model;
    protected NewMonsterMethod method;
    protected NewMonsterAnimator animator;

    protected NewMonsterStateMachine stateMachine;

    protected bool isStun;
    protected bool GetIsStun() => isStun;

    public bool isAttacking = false;

    private Rigidbody2D rigid;

    private Vector3 lastPlayerPos;

    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        method = GetComponent<NewMonsterMethod>();
        animator = GetComponent<NewMonsterAnimator>();
        rigid = GetComponent<Rigidbody2D>();
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
            new Sequence(BuildStunSequence()),
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

    protected virtual List<BTNode> BuildStunSequence()
    {
        return new List<BTNode>
        {
            new IsStunNode(GetIsStun()),
            new ActionNode(Idle)
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
    public void TakeDamage(float damage)
    {
        model.SetMonsterHp(-damage);
    }

    public void ReceiveKnockBack(Vector2 playerPos, Vector2 playerDir)
    {
        animator.PlayHit();
        
        string dirString = GetDirectionByAngle(player.transform.position, transform.position).ToString();
        Vector2 dir = Vector2.down;
        
        switch (dirString)
        {
            case "Left":
                dir = Vector2.left;
                break;
            
            case "Right":
                dir = Vector2.right;
                break;
            
            case "Up":
                dir = Vector2.up;
                break;
            
            case "Down":
                dir = Vector2.down;
                break;
            
            default:
                dir = Vector2.down;
                break;
        }
        
        rigid.AddForce(-dir * 5f, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// 방향 구해주는 메서드
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="monsterPos"></param>
    /// <returns></returns>
    private Direction GetDirectionByAngle(Vector2 playerPos, Vector2 monsterPos)
    {
        // 몬스터 위치에서 플레이어 위치로 향하는 벡터
        Vector2 dir = playerPos - monsterPos;

        // 몬스터의 정면을 Vector2.right로 가정 (0도)
        float angle = Vector2.SignedAngle(Vector2.right, dir);
    
        // 각도에 따라 4방위 판별
        if (angle > -45f && angle <= 45f) 
        {
            return Direction.Right;
        }
        else if (angle > 45f && angle <= 135f) 
        {
            return Direction.Up;
        }
        else if (angle > 135f || angle <= -135f) // 180도 처리
        {
            return Direction.Left;
        }
        else // angle > -135f && angle <= -45f
        {
            return Direction.Down;
        }
    }

    public void ReceiveDot(float duration, float tick, float damage)
    {
        StartCoroutine(DotCoroutine(duration, tick, damage));
    }

    private IEnumerator DotCoroutine(float duration, float tick, float damage)
    {
        // 코루틴 시작 시 첫 피해를 바로 적용
        model.SetMonsterHp(-damage);

        float elapsed = 0f; // 경과 시간

        // duration이 다 될 때까지 반복
        while (elapsed < duration)
        {
            // tick만큼 기다리기
            yield return new WaitForSeconds(tick);

            // 피해 적용
            model.SetMonsterHp(-damage);

            // 경과 시간 누적
            elapsed += tick;
        }
    }

    public void ReceiveStun(float duration)
    {
        animator.PlayHit();
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStun = true;
        
        yield return new WaitForSeconds(duration);
        
        isStun = false;
    }

    public void ReceiveSlow(float duration, float ratio)
    {
        StartCoroutine(SlowCoroutine(duration, ratio));
    }

    private IEnumerator SlowCoroutine(float duration, float ratio)
    {
        float preSpeed = model.MoveSpeed;

        model.MoveSpeed /= ratio;
        
        yield return new WaitForSeconds(duration);
        model.MoveSpeed = preSpeed;
    }

    public void ReceiveDefenseDeBuff(float duration, float deBuffPercent)
    {
        StartCoroutine(debuffDefCoroutine(duration, deBuffPercent));
    }

    private IEnumerator debuffDefCoroutine(float duration, float deBuffPercent)
    {
        float preDef = model.def;

        model.def /= deBuffPercent;
        
        yield return new WaitForSeconds(duration);
        
        model.def = preDef;
    }
}
