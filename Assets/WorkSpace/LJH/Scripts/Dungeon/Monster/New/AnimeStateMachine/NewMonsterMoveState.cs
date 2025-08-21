using UnityEngine;

public class NewMonsterMoveState : NewIMonsterState
{
    private Transform monsterTransform;
    private Transform playerTransform;

    public NewMonsterMoveState(Transform monsterT, Transform playerT)
    {
        this.monsterTransform = monsterT;
        this.playerTransform = playerT;
    }
    public void Enter(NewMonsterAnimator animator)
    {
        // 1. 플레이어를 향하는 방향 계산
        Vector2 directionToPlayer = playerTransform.position - monsterTransform.position;

        // 2. 애니메이터에 방향 전달
        animator.SetFacingByDelta(directionToPlayer);

        animator.PlayMove();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
