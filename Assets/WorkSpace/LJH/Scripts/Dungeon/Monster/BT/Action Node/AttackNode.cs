using System.Collections;
using UnityEngine;

public class AttackNode : BTNode
{
    private readonly System.Action performAttack;

    public AttackNode(System.Action performAttack)
    {
        this.performAttack = performAttack;
    }

    public override NodeState Tick()
    {
        //요놈이 세번 실행중
        Debug.Log("어택 노드 실행");
        performAttack?.Invoke();
        return NodeState.Success;
    }
}