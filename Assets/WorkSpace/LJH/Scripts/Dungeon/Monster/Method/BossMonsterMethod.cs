using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BossMonsterMethod : MonoBehaviour
{
    [Inject]
    private DungeonManager dungeonManager;

    [SerializeField] protected GameObject player;
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private BoxCollider2D rangeCollider;
    
    protected Coroutine moveCoroutine;
    protected AstarPath astarPath;
    protected Rigidbody2D rb;
    
    public bool isMoving;

    public void PlayerInit(GameObject player)
    {
        this.player = player;
    }
    public void MonsterDataInit(MonsterData data)
    {
        monsterData = data;
        if (monsterData == null)
        {
            Debug.LogWarning("MonsterData is null");
        }
    }

    public void MonsterInit()
    {
        astarPath = GetComponentInChildren<AstarPath>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        if (astarPath.path == null)
        {
            Debug.Log("astar패스가 널임");
        }
        if (astarPath.path != null && astarPath.path.Count > 1)
        {
            for (int i = 1; i < astarPath.path.Count; i++)
            {
                Vector2 target = astarPath.path[i];

                while (Vector2.Distance(rb.position, target) > 0.01f)
                {
                    Vector2 current = rb.position;
                    Vector2 direction = target - current;

                    Vector2 moveDir = Vector2.zero;

                    if (Mathf.Abs(direction.x) > 0.01f)
                    {
                        moveDir = (direction.x > 0) ? Vector2.right : Vector2.left;
                    }
                    else if (Mathf.Abs(direction.y) > 0.01f)
                    {
                        moveDir = (direction.y > 0) ? Vector2.up : Vector2.down;
                    }
                    else
                    {
                        break;
                    }

                    Vector2 nextPos = current + moveDir * monsterData.MoveSpeed * Time.fixedDeltaTime;

                    // Clamp: 목표를 넘어가지 않도록
                    if (Vector2.Distance(nextPos, target) > Vector2.Distance(current, target))
                    {
                        nextPos = target;
                    }

                    rb.MovePosition(nextPos);
                    yield return new WaitForFixedUpdate();
                }

                rb.MovePosition(target); // 목표 위치 스냅
                yield return new WaitForSeconds(0.05f);
            }
        }

        isMoving = false;
        moveCoroutine = null;
    }

    public void StopMoveCo()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
            isMoving = false;
        }
    }

    public void Move()
    {
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }

            moveCoroutine = StartCoroutine(MoveCoroutine());
            
        }
    }

    public void Attack()
    {
        Debug.Log("공격");
        StopMoveCo();
    }

    public void BeforeAttack()
    {
        if (rangeCollider != null)
        {
            rangeCollider.enabled = true;
        }
    }

    public void AfterAttack()
    {
        if (rangeCollider != null)
        {
            rangeCollider.enabled = false;
        }
    }

    public abstract void Skill_First();
    public abstract void Skill_Second();

    public void Die()
    {
        DropItem();
        Debug.Log("Boss died");
    }

    private void DropItem()
    {
        // Implement drop logic
    }
    
    protected virtual void SetPosEffect(GameObject effect)
    {
        effect.transform.position = player.transform.position;
    }

    protected virtual void SetEffectActiver(GameObject effect)
    {
        effect.SetActive(true);
    }
}