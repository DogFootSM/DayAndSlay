using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackNode : BTNode
{
    private System.Action performAttack;
    private Animator animator;
    private string animeName;
    private bool isPlayed = false;

    public AttackNode(System.Action performAttack, Animator animator, string animeName)
    {
        this.performAttack = performAttack;
        this.animator = animator;
        this.animeName = animeName;
    }

    public override NodeState Tick()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        int expectedHash = Animator.StringToHash("Base Layer.Monster Attack Left");

        if (!isPlayed)
        {
            Debug.Log("애니메이션 시작");
            performAttack?.Invoke(); // animator.Play(animeName);
            isPlayed = true;
            return NodeState.Running;
        }

        // 실제 애니메이션 상태 진입할 때까지 기다림
        if (stateInfo.fullPathHash != expectedHash)
        {
            Debug.Log("아직 해당 상태로 진입 안됨");
            return NodeState.Running;
        }

        if (stateInfo.normalizedTime >= 1f)
        {
            Debug.Log("애니메이션 완료");
            isPlayed = false;
            return NodeState.Success;
        }

        Debug.Log($"진행 중: {stateInfo.normalizedTime}");
        return NodeState.Running;
    }
}
