using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GeneralMonsterMethod : MonoBehaviour
{
    private MonsterData monsterData;
    [Inject] private DungeonManager dungeonManager;
    [SerializeField] private GeneralAnimator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D attackHitBox;
    [SerializeField] private AstarPath astarPath;
    [SerializeField] private GameObject player;
    
    [SerializeField] private float repathInterval = 0.6f;      // 리패스 최소 주기(초)
    [SerializeField] private float targetMovedThreshold = 0.6f; // 플레이어가 이만큼 움직이면 리패스
    private Vector2 lastRepathTarget;
    private float lastRepathTime;
    
    private MonsterModel monster;

    
    private Coroutine idleCoroutine;
    private Coroutine moveCoroutine;
    private bool isIdling;          
    private bool isDead = false;

    private const float ARRIVE_EPS = 0.05f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<GeneralAnimator>();
        player = GameObject.FindWithTag("Player");
        monster = GetComponent<MonsterModel>();
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
    /// Idle : 근처를 랜덤하게 1~2칸 이동
    /// </summary>
    public void IdleMethod()
    {
        if (isIdling) return;           // 이미 Idle 대기 중이면 아무것도 안 함
        if (moveCoroutine != null)      // 이동 중이었다면 이동만 정지
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        idleCoroutine = StartCoroutine(IdleRoutine());
    }

    private IEnumerator IdleRoutine()
    {
        isIdling = true;
        animator?.PlayIdle();

        // 2~5초 랜덤 대기
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);

        // 랜덤 오프셋 (0,0 방지)
        Vector2 currentPos = rb.position;
        Vector2 randomOffset;
        do
        {
            randomOffset = new Vector2(Random.Range(-2, 3), Random.Range(-2, 3));
        } while (randomOffset == Vector2.zero);

        var pathSnapshot = new List<Vector2> { currentPos, currentPos + randomOffset };

        // 이동 시작 (이동 코루틴 핸들만 사용)
        moveCoroutine = StartCoroutine(MoveCoroutine(pathSnapshot));

        // 이동 코루틴이 끝날 때까지 대기
        while (moveCoroutine != null) yield return null;

        // 한 사이클 끝 → 다음 IdleMethod 호출을 허용
        isIdling = false;
        idleCoroutine = null;
    }
    
    public void StopIdle()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
        isIdling = false;
    }

    

    /// <summary>
    /// Chase : 플레이어 감지 시 경로 따라 추적
    /// </summary>
    public void MoveMethod()
    {
        if (isDead) return;

        if (astarPath == null || astarPath.path == null || astarPath.path.Count < 2)
            return;

        StopMoveCo();

        // 경로 스냅샷
        List<Vector2> pathSnapshot = new List<Vector2>();
        foreach (Vector3 p in astarPath.path)
            pathSnapshot.Add(p);

        moveCoroutine = StartCoroutine(MoveCoroutine(pathSnapshot));
    }
    

    /// <summary>
    /// Attack : 사거리 안에 플레이어가 있으면 공격
    /// </summary>
    public void AttackMethod()
    {
        if (isDead) return;
        
        StopMoveCo();

        Vector2 toPlayer = player.transform.position - transform.position;
        animator?.SetFacingByDelta(toPlayer);
        animator?.PlayAttack();

        // Todo: 실제 Player HP 처리
        Debug.Log($"몬스터가 플레이어에게 {monsterData.Attack} 피해를 입힘");
    }

    /// <summary>
    /// Die : 사망 처리
    /// </summary>
    public void DieMethod()
    {
        if (isDead) return;
        isDead = true;

        StopMoveCo();
        animator?.PlayDie();

        DropItem();
        Destroy(gameObject, 1.0f); // 1초 후 파괴
    }

    // ==============================
    // 이동 코루틴
    // ==============================

    private IEnumerator MoveCoroutine(List<Vector2> path)
    {
        yield return new WaitForFixedUpdate();
        animator?.PlayMove();

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 target = path[i];
            while (Vector2.Distance(rb.position, target) > ARRIVE_EPS)
            {
                Vector2 current = rb.position;
                Vector2 delta = target - current;

                Vector2 stepDir = (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    ? ((delta.x > 0) ? Vector2.right : Vector2.left)
                    : ((delta.y > 0) ? Vector2.up : Vector2.down);

                animator?.SetFacingByDelta(stepDir);

                float step = monsterData.MoveSpeed * Time.fixedDeltaTime;
                Vector2 nextPos = current + stepDir * step;

                if ((target - nextPos).sqrMagnitude > (target - current).sqrMagnitude)
                    nextPos = target;

                rb.MovePosition(nextPos);
                yield return new WaitForFixedUpdate();
            }

            rb.MovePosition(target);
            yield return new WaitForFixedUpdate();
        }

        animator?.PlayIdle();
        moveCoroutine = null;
    }

    public void StopMoveCo()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    // ==============================
    // 기타 유틸
    // ==============================

    public void HitMethod(int damage)
    {
        monster.SetMonsterHp(damage);
        if (monster.GetMonsterHp() <= 0)
            DieMethod();
    }

    private void DropItem()
    {
        DropItemEntry dropEntry = monsterData.DropTable[Random.Range(0, monsterData.DropTable.Count)];
        int ItemId = dropEntry.ItemId;

        ItemData itemData = ItemDatabaseManager.instance.GetItemByID(ItemId);
        if (itemData == null)
        {
            Debug.LogWarning("드랍할 아이템을 찾을 수 없습니다.");
            return;
        }

        GameObject dropItem = dungeonManager.pool.GetPool();
        dropItem.GetComponent<Item>().itemData = itemData;
        dropItem.transform.position = transform.position;
    }
}
