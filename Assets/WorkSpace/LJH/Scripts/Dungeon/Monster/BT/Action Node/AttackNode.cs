using System.Collections;
using UnityEngine;

public class AttackNode : BTNode
{
    private readonly System.Action performAttack;
    private readonly GeneralMonsterAI ai;

    public AttackNode(System.Action performAttack)
    {
        this.performAttack = performAttack;
    }

    public override NodeState Tick()
    {

        performAttack?.Invoke();
        return NodeState.Success;
    }
}