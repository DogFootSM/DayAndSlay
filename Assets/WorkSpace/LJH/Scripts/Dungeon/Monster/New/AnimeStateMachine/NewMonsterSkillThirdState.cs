using UnityEngine;

public class NewMonsterSkillThirdState : NewIMonsterState
{
    private Transform monsterTransform;
    private Transform playerTransform;

    public NewMonsterSkillThirdState(Transform monsterT, Transform playerT)
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
        animator.PlaySkillThird();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
