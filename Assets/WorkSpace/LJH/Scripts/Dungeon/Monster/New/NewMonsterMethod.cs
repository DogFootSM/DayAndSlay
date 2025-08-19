using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NewMonsterMethod : MonoBehaviour
{
    private MonsterData monsterData;

    private void Start()
    {
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
    }
    // ==============================
    // 이동 코루틴
    // ==============================

    #region Move
    private IEnumerator MoveCoroutine(List<Vector2> path)
    {
        yield return null;
    }

    public void StopMoveCo()
    {
        
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
