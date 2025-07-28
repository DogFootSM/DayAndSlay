using UnityEngine;

public class BossMonsterIdleState : IBossMonsterState
{

    public void Enter(BossAnimator animator)
    {
        Debug.Log("아이들 실행");
        animator.PlayIdle();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
