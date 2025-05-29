using System.Collections;
using UnityEngine;

public class AttackNode : BTNode
{
    private readonly System.Action performAttack;
    private readonly Animator animator;
    private readonly string animationStateName;
    private readonly GeneralMonsterAI ai;

    private bool hasStarted = false;
    private bool waitingForStateEnter = false;

    public AttackNode(System.Action performAttack, Animator animator, string animationStateName, GeneralMonsterAI ai)
    {
        this.performAttack = performAttack;
        this.animator = animator;
        this.animationStateName = animationStateName;
        this.ai = ai;
    }

    public override NodeState Tick()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!hasStarted)
        {
            Debug.Log("공격 명령 시작");
            ai.isAttacking = true;
            performAttack?.Invoke();
            hasStarted = true;
            waitingForStateEnter = true;
            return NodeState.Running;
        }

        if (waitingForStateEnter)
        {
            if (!stateInfo.IsName(animationStateName))
            {
                Debug.Log("상태가 이미 바뀌어서 normalizedTime 못 보고 있음");
            }

            if (stateInfo.IsName(animationStateName))
            {
                Debug.Log("애니메이션 상태 진입 완료");
                waitingForStateEnter = false;
            }
            else
            {
                Debug.Log("애니메이션 상태 진입 대기 중...");
                return NodeState.Running;
            }
        }

        if (stateInfo.normalizedTime >= 1f)
        {
            Debug.Log("애니메이션 완료");
            hasStarted = false;
            ai.isAttacking = false;
            return NodeState.Success;
        }

        Debug.Log($"애니메이션 진행 중: {stateInfo.normalizedTime:F2}");
        return NodeState.Running;
    }
}