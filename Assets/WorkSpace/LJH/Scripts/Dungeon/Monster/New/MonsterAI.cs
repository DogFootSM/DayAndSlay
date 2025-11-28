using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour, IEffectReceiver
{
    [SerializeField] private GameObject markObject;
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected PlayerController player;
    
    protected BehaviourTree tree;
    protected MonsterModel model;
    protected MonsterMethod method;
    protected MonsterAnimator animator;

    protected NewMonsterStateMachine stateMachine;
    
    //데미지 및 효과 처리 변수
    protected Coroutine knockBackCo;
    protected Coroutine dotDurationCo;
    protected Coroutine dotDamageCo;
    protected Coroutine stunCo;
    protected Coroutine slowCo;
    protected Coroutine defenseDeBuffCo;
    
    protected Vector2 knockBackDir;

    protected bool isSlow;
    protected bool isStun = false;
    protected bool isDot;
    protected bool isDefenseDeBuffed;

    protected float knockBackPower = 5f;
    private float defenseDeBuffRatio;
    public bool GetIsStun() => isStun;
    public bool IsSlow => isSlow;
    
    public bool isAttacking = false;
    public bool isSkillUsing = false;

    private Rigidbody2D rigid;

    private Vector3 lastPlayerPos;
 
    protected virtual void Awake()
    {
        model = GetComponent<MonsterModel>();
        method = GetComponent<MonsterMethod>();
        animator = GetComponent<MonsterAnimator>();
        rigid = GetComponent<Rigidbody2D>();
        lastPlayerPos = transform.position;
        //markObject.SetActive(false);
    }

    protected virtual void Start()
    {
        StartCoroutine(StartDelayCoroutine());
    }

    private IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        
        //Tag로 검색되는 오브젝트가 PlayerRoot로 바뀌어 겟컴포칠드런으로 플레이어 찾아줌
        player = GameObject.FindWithTag("Player")?.GetComponentInChildren<PlayerController>();
        
        method.SetPlayer(player.gameObject);
        
        tree = new BehaviourTree(BuildRoot());

        stateMachine = new NewMonsterStateMachine(animator);

        method.MonsterDataInit(monsterData);
    }

    protected virtual void Update()
    {   
        if (GetIsStun()) return;
        
        if (isSkillUsing) return;
        
        if (tree == null) return;
        tree.Tick();

        //테스트 시에만 주석 해제
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ReceiveKnockBack(player.transform.position, Vector2.zero);
        //}
        
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
            new IsDieNode(() => model.CurHp),
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
    }

    protected IEnumerator AttackEndDelay()
    {
        yield return new WaitForSeconds(model.AttackCooldown);
    
        isAttacking = false;
    }

    public MonsterData GetMonsterData() => monsterData;
    public MonsterModel GetMonsterModel() => model;
    public MonsterMethod GetMonsterMethod() => method;
    public MonsterAnimator GetMonsterAnimator() => animator;
    
    public float GetChaseRange() => monsterData.ChaseRange;
    public void TakeDamage(float damage)
    {
        float calcDefense = model.def;
        
        //TODO: 임시 디버프 방어력 계산
        if (isDefenseDeBuffed)
        {
            calcDefense -= CalculateDefenseDeBuff();
        }

        model.SetMonsterHp(-damage);
    }
    /// <summary>
    /// 디버프에 따른 방어력 계산
    /// </summary>
    /// <returns></returns>
    private float CalculateDefenseDeBuff()
    {
        return model.def * defenseDeBuffRatio;
    }


    /// <summary>
    /// 현재 몬스터 최대 체력 반환
    /// </summary>
    /// <returns></returns>
    public float GetMaxHp()
    {
        return model.GetMonsterMaxHp();
    }

    /// <summary>
    /// 현재 몬스터 방어력 반환
    /// </summary>
    /// <returns></returns>
    public float GetDefense()
    {
        return model.def;
    }

    public void ReceiveMarkOnTarget()
    {
        markObject.SetActive(!markObject.activeSelf);
    }
    
    /// <summary>
    /// 공격력 감소 디버프
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="deBuffPer"></param>
    public void ReceiveAttackDeBuff(float duration, float deBuffPer)
    {
        AttackDeBuff(duration, deBuffPer);
    }

    protected virtual void AttackDeBuff(float duration, float deBuffPer)
    {
        //몬스터 특성에 따라 공격력 디버프 감소 설정
    }
    
    #region 넉백 효과
    /// <summary>
    /// PlayerDir 사용하지 않아서 아무값이나 넣어줌 _ 백선명한테 말해서 아예 빼버려야함
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="playerDir"></param>
    public void ReceiveKnockBack(Vector2 playerPos)
    {
        animator.PlayHit();
        
        //벡터로 계산해버리면?
        knockBackDir = ((Vector2)transform.position - playerPos);
        

        if (knockBackCo != null)
        {
            StopCoroutine(knockBackCo);
            knockBackCo = null;
        }

        knockBackCo = StartCoroutine(KnockBackRoutine());
    }
    /// <summary>
    /// 넉백 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator KnockBackRoutine()
    {
        rigid.AddForce(knockBackDir * knockBackPower, ForceMode2D.Impulse);

        yield return WaitCache.GetWait(0.3f);

        rigid.velocity = Vector2.zero;
    }
    #endregion

    #region 도트 데미지
    public void ReceiveDot(float duration, float tick, float damage)
    {
        StartCoroutine(DotCoroutine(duration, tick, damage));
        
        if (dotDurationCo != null)
        {
            StopCoroutine(dotDurationCo);
            dotDurationCo = null;
        }

        if (dotDamageCo != null)
        {
            StopCoroutine(dotDamageCo);
            dotDamageCo = null;
        }
        
        dotDurationCo = StartCoroutine(DotDurationRoutine(duration));
        dotDamageCo = StartCoroutine(DotDamageRoutine(tick, damage));
    }
    
    /// <summary>
    /// 도트 지속 시간 코루틴
    /// </summary>
    /// <param name="duration"></param>
    protected virtual IEnumerator DotDurationRoutine(float duration)
    {
        float elapsedTime = 0f;

        isDot = true;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isDot = false;
    }
    
    /// <summary>
    /// 도트 데미지 틱 계산 코루틴
    /// </summary>
    /// <param name="tick"></param>
    /// <param name="perSecondDamage"></param>
    /// <returns></returns>
    protected virtual IEnumerator DotDamageRoutine(float tick, float perSecondDamage)
    {
        while (isDot)
        {
            yield return WaitCache.GetWait(tick);
            TakeDamage(perSecondDamage);
        }
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
    
    #endregion
    
    #region 기절 효과
    public void ReceiveStun(float duration)
    {
        animator.PlayStun(duration);
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        float elapsedTime = 0f;

        isStun = true;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        isStun = false;
    }
    
    #endregion

    #region 둔화 효과
    public void ReceiveSlow(float duration, float ratio)
    {
        StartCoroutine(SlowCoroutine(duration, ratio));
    }

    protected virtual IEnumerator SlowCoroutine(float duration, float ratio)
    {
        float elapsedTime = 0f;
        isSlow = true;

        float originMoveSpeed = model.MoveSpeed;

        model.MoveSpeed -= (model.MoveSpeed * ratio);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isSlow = false;
        model.MoveSpeed = originMoveSpeed;
    }
    #endregion

    #region 방어력 감소 효과
    public void ReceiveDefenseDeBuff(float duration, float deBuffPercent)
    {
        defenseDeBuffRatio = deBuffPercent;
        
        DefenseDeBuff(duration);
    }
 
    protected virtual void DefenseDeBuff(float duration)
    {
        if (defenseDeBuffCo != null)
        {
            StopCoroutine(defenseDeBuffCo);
            defenseDeBuffCo = null;
        }

        defenseDeBuffCo = StartCoroutine(DefenseDeBuffRoutine(duration));
    }

    protected virtual IEnumerator DefenseDeBuffRoutine(float duration)
    {
        float elaspedTime = 0f;
    
        isDefenseDeBuffed = true;
        
        while (elaspedTime < duration)
        {
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        isDefenseDeBuffed = false;
    }
    #endregion
    
    
    
    
    
    /// <summary>
    /// 방향 구해주는 메서드
    /// </summary>
    /// <param name="playerPos"></param>
    /// <param name="monsterPos"></param>
    /// <returns></returns>
    public Direction GetDirectionByAngle(Vector2 playerPos, Vector2 monsterPos)
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
}
