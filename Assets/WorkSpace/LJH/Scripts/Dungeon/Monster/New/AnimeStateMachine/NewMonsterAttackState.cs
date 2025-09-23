using UnityEngine;

public class NewMonsterAttackState : NewIMonsterState
{
    private Transform monsterTransform;
    private Transform playerTransform;

    public NewMonsterAttackState(Transform monsterT, Transform playerT)
    {
        this.monsterTransform = monsterT;
        this.playerTransform = playerT;
    }

    public void Enter(MonsterAnimator animator)
    {
        // 1. 플레이어를 향하는 방향 계산
        Vector2 directionToPlayer = playerTransform.position - monsterTransform.position;

        // 2. 애니메이터에 방향 전달
        animator.SetFacingByDelta(directionToPlayer);

        // 3. 공격 애니메이션 재생
        animator.PlayAttack();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
