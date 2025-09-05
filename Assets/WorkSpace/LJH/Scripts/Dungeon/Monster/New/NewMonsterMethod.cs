using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NewMonsterMethod : MonoBehaviour
{
    protected MonsterData monsterData;
    protected AstarPath astarPath;
    protected NewMonsterAI ai;
    protected NewMonsterAnimator animator;
    protected Rigidbody2D rb;
    protected TargetSensor sensor;
    protected GameObject player;
    protected Collider2D coll;
    
    private int currentPathIndex;

    private List<Vector3> path = new List<Vector3>();

    protected Vector2 dir;
    
    public void SetPlayer(GameObject player) => this.player = player;

    public virtual void Skill_First() { }

    public virtual void Skill_Second() { }
    public virtual void Skill_Third() { }
    public virtual void Skill_Fourth() { }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        astarPath = GetComponentInChildren<AstarPath>();
        ai = GetComponentInChildren<NewMonsterAI>();
        animator = ai.GetMonsterAnimator();
        sensor = GetComponentInChildren<TargetSensor>();
        coll = GetComponent<Collider2D>();
        MonsterDataInit(ai.GetMonsterData());
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
    
    public virtual void Move()
    {
        // 경로가 비어있으면 A*에서 경로를 가져와서 시작합니다.
        // 이렇게 하면 몬스터가 경로를 받기 전까지 멈춰있게 됩니다.
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
                // A*가 아직 경로를 계산 중이면 그냥 리턴합니다.
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

        rb.MovePosition(Vector3.MoveTowards(transform.position, targetWorldPos, monsterData.MoveSpeed * Time.deltaTime));

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            currentPathIndex++;
        }
    }
    
    public virtual void RequestPathUpdate(Vector3 start, Vector3 end)
    {
        // Debug.Log("경로 갱신 요청");
        // A* Pathfinding 실행
        astarPath.DetectTarget(start, end);
    }

    #endregion

    /// <summary>
    /// Attack : 사거리 안에 플레이어가 있으면 공격
    /// </summary>
    public void AttackMethod()
    {
        // Todo: 실제 Player HP 처리
        //Debug.Log($"몬스터가 플레이어에게 {monsterData.Attack} 피해를 입힘");

        Direction direction = ai.GetDirectionByAngle(player.transform.position, transform.position);
        
        float distance = Vector2.Distance(player.transform.position, transform.position);

        //몬스터가 보는 방향과 플레이어와의 방향 비교)
        if (animator.GetCurrentDir() == direction)
        {
            //사거리 비교
            if (distance <= ai.GetMonsterModel().AttackRange)
            {
                Debug.Log("몬스터 공격");
            }
        }

    }

    /// <summary>
    /// Die : 사망 처리
    /// </summary>
    public void DieMethod()
    {
        Debug.Log("사망");
    }


    // ==============================
    // 기타 유틸
    // ==============================

    public void HitMethod(int damage)
    {
        Debug.Log("피격");
        ai.ReceiveKnockBack(player.transform.position, Vector2.left);
    }

    protected void DropItem()
    {
        Debug.Log("아이템 떨어뜨림");
    }
}
