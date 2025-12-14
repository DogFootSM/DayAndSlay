using UnityEngine;

public class NewMonsterMoveState : NewIMonsterState
{
    private Transform monsterTransform;

    private Transform playerTransform;   // Chase遂
    private Vector3? targetPosition;     // Idle遂

    // Chase 持失切
    public NewMonsterMoveState(Transform monsterT, Transform targetT)
    {
        monsterTransform = monsterT;
        playerTransform = targetT;
        targetPosition = null;
    }

    // Idle 持失切
    public NewMonsterMoveState(Transform monsterT, Vector3 targetPos)
    {
        monsterTransform = monsterT;
        playerTransform = null;
        targetPosition = targetPos;
    }
    
    private Vector3 GetTargetPosition()
    {
        if (playerTransform != null)
            return playerTransform.position;

        if (targetPosition.HasValue)
            return targetPosition.Value;

        return monsterTransform.position;
    }
    public void Enter(MonsterAnimator animator)
    {
        Vector3 targetPos = GetTargetPosition();
        
        Vector2 delta = targetPos - monsterTransform.position;

        animator.SetFacingByDelta(delta);
        
        animator.PlayMove();
    }
    public void Update()
    { 
    }
    public void Exit()
    {
    }
}
