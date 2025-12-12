using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterMethod : MonoBehaviour
{
    protected MonsterSound sound;
    
    
    [SerializeField] private int tempDamage;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject healPack;
    
    private DamageEffect damageEffect;
    
    protected MonsterData monsterData;
    protected MonsterModel model;
    protected AstarPath astarPath;
    [SerializeField] protected MonsterAI ai;
    protected MonsterAnimator animator;
    protected Rigidbody2D rb;
    protected TargetSensor sensor;
    protected GameObject player;
    protected PlayerController _player;
    protected Collider2D coll;
    protected bool isDead = false;
    
    private int currentPathIndex;

    private List<Vector3> path = new List<Vector3>();
    
    private HitController hitController;
    private HitEffectPool _hitEffect => HitEffectPool.Instance;

    
    public void SetPlayer(GameObject player) => this.player = player;

    public virtual void Skill_First() { }

    public virtual void Skill_Second() { }
    public virtual void Skill_Third() { }
    public virtual void Skill_Fourth() { }

    protected int parryingCount;
    
    //테스트용
    [SerializeField] private GameObject stunImage;
    
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        astarPath = GetComponentInChildren<AstarPath>();
        ai = GetComponent<MonsterAI>();
        animator = ai.GetMonsterAnimator();
        sensor = GetComponentInChildren<TargetSensor>();
        coll = GetComponent<Collider2D>();
        MonsterDataInit(ai.GetMonsterData());
        damageEffect = GetComponent<DamageEffect>();
        model = ai.GetMonsterModel();
        sound = GetComponent<MonsterSound>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            HitMethod(tempDamage);
            //animator.PlayHit();
        }

        if (!stunImage.activeSelf && ai.GetIsStun())
        {
            stunImage.SetActive(true);
        }
        else if (stunImage.activeSelf && !ai.GetIsStun())
        {
            stunImage.SetActive(false);
        }
    }


    public void MonsterDataInit(MonsterData monsterData)
    {
        this.monsterData = monsterData;
        if (this.monsterData == null)
            Debug.Log("몬스터데이터 주입 안됨");
    }

    // ==============================
    // 상태별 동작
    // ==============================

    /// <summary>
    /// Todo : Idle : 근처를 랜덤하게 1~2칸 이동
    /// </summary>
    public void IdleMethod()
    {
        //Todo : asdf
    }

    #region Idle
    
    private IEnumerator IdleRoutine()
    {
        yield return null;
    }
    
    public void StopIdle()
    {
        
    }

    #endregion

    /// <summary>
    /// Chase : 플레이어 감지 시 경로 따라 추적
    /// </summary>
    public void MoveMethod()
    {
        Move();
    }
    // ==============================
    // 이동 코루틴
    // ==============================

    #region Move
    // AI 스크립트에서 호출될 이동 함수
    
    public void Move()
    {
        if (path == null || path.Count == 0)
        {
            if (astarPath.path != null && astarPath.path.Count > 0)
            {
                // A*가 경로 계산을 완료했으면 가져와서 이동 시작
                path = astarPath.path;
                currentPathIndex = 0;
            }
            else
            {
                // A*가 아직 경로를 계산 중이면 리턴
                return;
            }
        }
        
        // 경로가 이미 있으면 다음 지점으로 이동
        if (currentPathIndex >= path.Count)
        {
            // 경로 끝에 도달했으므로 경로 초기화
            path.Clear();
            currentPathIndex = 0;
            return;
        }

        Vector3 targetGridPos = path[currentPathIndex];
        Vector3 targetWorldPos = new Vector3(targetGridPos.x, targetGridPos.y, 0);
        
        //IsAction을 강제로 false로 초기화
        animator.SetIsAction(false);

        rb.MovePosition(Vector3.MoveTowards(transform.position, targetWorldPos, monsterData.MoveSpeed * Time.deltaTime));
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            currentPathIndex++;
        }
    }
    
    public virtual void RequestPathUpdate(Vector3 start, Vector3 end)
    {
        // A* Pathfinding 실행
        astarPath.DetectTarget(start, end);
    }

    #endregion

    /// <summary>
    /// Attack : 사거리 안에 플레이어가 있으면 공격
    /// 해당 메서드는 애니메이션 이벤트로 호출 >> 공격 타이밍 떄문에
    /// </summary>
    public virtual void AttackMethod()
    {
        sound.PlaySFX(SoundType.ATTACK);
        
        Direction direction = ai.GetDirectionByAngle(player.transform.position, transform.position);
        
        float distance = Vector2.Distance(player.transform.position, transform.position);

        //몬스터가 보는 방향과 플레이어와의 방향 비교)
        if (animator.GetCurrentDir() == direction)
        {
            //사거리 비교
            if (distance <= ai.GetMonsterModel().AttackRange)
            {
                PlayerHpDamaged(monsterData.Attack);
            }
        }

    }

    /// <summary>
    /// 실제 공격 함수
    /// </summary>
    /// <param name="damage"></param>
    protected void PlayerHpDamaged(int damage)
    {
        if(_player == null) _player = player.GetComponent<PlayerController>();
        
        _player.TakeDamage(ai, damage);
    }

    /// <summary>
    /// Die : 사망 처리
    /// </summary>
    public virtual void DieMethod()
    {
        if (isDead) return;
        
        isDead = true;
        animator.PlayDie();
        sound.PlaySFX(SoundType.DEATH);
        StartCoroutine(MonsterDestroyRoutine());
    }

    private IEnumerator MonsterDestroyRoutine()
    {
        yield return new WaitForSeconds(1f);
        DropHealPack();
        DropItem();
        GainExp(monsterData.Experience);
        Destroy(gameObject);
    }

    protected void GainExp(int monsterExp)
    {
        player.GetComponent<PlayerModel>().GainExperience(monsterExp);
    }


    // ==============================
    // 기타 유틸
    // ==============================

    public void HitMethod(float damage)
    {
        //몬스터가 죽은 경우 피격되지 않게 처라
        if (isDead) return;
        
        sound.PlaySFX(SoundType.HIT);
        damageEffect.DamageTextEvent(damage);
        if(hitController == null)
            hitController = GetComponent<HitController>();
        
        hitController.ActiveHitEffect(damage);
    }

    
    public virtual void StunMethod(float duration)
    {
        ai.ReceiveStun(duration);
    }

    public void DropItem()
    {
        float randomNum = Random.Range(0, 100);
        
        //노드랍인경우 여기서 리턴
        if (model.DropItemPick(randomNum) == null) return;
        
        itemPrefab.GetComponent<Item>().SetItem(model.DropItemPick(randomNum));
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }

    private void DropHealPack()
    {
        float randomNum = Random.Range(0, 3);
        
        if (randomNum == 2) return;
        
        int healAmountMin = Mathf.FloorToInt(model.MaxHp * 0.1f);
        int healAmountMax = Mathf.FloorToInt(model.MaxHp * 0.2f);
        int healAmount = Random.Range(healAmountMin, healAmountMax);
        
        GameObject healObj = Instantiate(healPack, transform.position, Quaternion.identity);
        healObj.GetComponent<HealthPack>().SetRecoveryAmount(healAmount);
    }
    
    
    /// <summary>
    /// 모든 몬스터에서 사용할 수 있도록 최상위 클래스에 배치해둔 헬퍼 함수
    /// </summary>
    /// <param name="monsterPos"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Direction GetDirectionToTarget(Vector3 monsterPos, Vector3 targetPos)
    {
        Vector3 directionVector = targetPos - monsterPos;

        if (Mathf.Abs(directionVector.y) > Mathf.Abs(directionVector.x))
        {
            // 상/하 결정
            if (directionVector.y > 0)
            {
                return Direction.Up; // Y 값이 양수: 위쪽
            }
            else
            {
                return Direction.Down; // Y 값이 음수: 아래쪽
            }
        }
        else
        {
            // 좌/우 결정
            if (directionVector.x > 0)
            {
                return Direction.Right; // X 값이 양수: 오른쪽
            }
            else
            {
                return Direction.Left; // X 값이 음수: 왼쪽
            }
        }
    }
}
