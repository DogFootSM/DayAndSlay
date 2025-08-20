using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NewMonsterMethod : MonoBehaviour
{
    private MonsterData monsterData;
    private AstarPath astarPath;
    private NewMonsterAI ai;
    private NewMonsterAnimator animator;
    private Rigidbody2D rb;
    
    public float moveSpeed = 5f;
    private int currentPathIndex;

    private List<Vector3> path = new List<Vector3>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        astarPath = GetComponentInChildren<AstarPath>();
        ai = GetComponentInChildren<NewMonsterAI>();
        animator = ai.GetMonsterAnimator();
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
        Debug.Log("대기");
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
        Debug.Log("이동");
        Move();
    }
    // ==============================
    // 이동 코루틴
    // ==============================

    #region Move
    // AI 스크립트에서 호출될 이동 함수
    public void Move()
    {
        Debug.Log("이동 함수 실행");
        // 경로가 비어있거나 이동이 이미 끝났으면 아무것도 하지 않습니다.
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
        {
            Debug.Log("무브 리턴되고있음");
            return;
        }

        // 현재 목표 지점의 그리드 좌표를 가져옵니다.
        Debug.Log(currentPathIndex);
        Debug.Log(path[currentPathIndex]);
        
        Vector3 targetGridPos = path[currentPathIndex];
        // 그리드 좌표를 월드 좌표로 변환합니다.
        Vector3 targetWorldPos = new Vector3(targetGridPos.x, targetGridPos.y, 0);

        Debug.Log($"[디버그] 현재 위치: {transform.position}, 목표 위치: {targetWorldPos}, 이동 속도: {moveSpeed}");
    
        // 2. Vector3.MoveTowards()의 반환값을 변수로 받아서 확인
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
        Debug.Log($"[디버그] 계산된 다음 위치: {nextPosition}");

        // 3. 이동 전후의 위치를 비교
        Vector3 positionBeforeMove = transform.position;
        
        // 목표 지점을 향해 몬스터를 이동시킵니다.
        // Vector3.MoveTowards를 사용하면 지정된 속도만큼 정확하게 이동합니다.

        rb.MovePosition(Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime));
        
        Vector3 positionAfterMove = transform.position;
        Debug.Log($"[디버그] 이동 전: {positionBeforeMove}, 이동 후: {positionAfterMove}");
        
        
        // 만약 거리가 매우 가깝다면 다음 경로 지점으로 인덱스를 이동시킵니다.
        if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
        {
            currentPathIndex++;

            // 만약 마지막 지점에 도달했다면 경로를 초기화합니다.
            if (currentPathIndex >= path.Count)
            {
                path.Clear();
                currentPathIndex = 0;
                // 이동이 끝났으므로 AI에 다음 상태로 전환하라고 알릴 수 있습니다.
                // 예: OnPathCompleted 이벤트 호출
            }
        }
    }

    // AI 스크립트에서 새로운 경로를 할당할 때 호출하는 함수
    public void SetNewPath()
    {
        //path.Clear();

        path = astarPath.path;
        
        currentPathIndex = 0;
    }

    #endregion

    /// <summary>
    /// Attack : 사거리 안에 플레이어가 있으면 공격
    /// </summary>
    public void AttackMethod()
    {
        Debug.Log("공격");
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
    }

    private void DropItem()
    {
        Debug.Log("아이템 떨어뜨림");
    }
}
