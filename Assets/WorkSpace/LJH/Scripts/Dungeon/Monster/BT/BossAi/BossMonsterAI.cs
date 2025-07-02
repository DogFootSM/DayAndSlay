using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(BossMonsterMethod))]
[RequireComponent(typeof(BossAnimator))]
[RequireComponent (typeof(MonsterModel))]
public abstract class BossMonsterAI : MonoBehaviour
{
    [Inject] protected TestPlayer player;
    [SerializeField] protected MonsterData monsterData;
    [SerializeField] protected BossAnimator animator;
    [SerializeField] protected BossMonsterMethod method;
    [SerializeField] protected MonsterModel model;

    protected BehaviourTree tree;

    protected virtual void Awake()
    {
        tree = new BehaviourTree(BuildRoot());
    }

    protected virtual void Update()
    {
        tree.Tick();

    }

    protected virtual BTNode BuildRoot()
    {
        return new Selector(new List<BTNode>
        {
            new Sequence(BuildAttackSequence()),
            new Sequence(BuildChaseSequence()),
            new ActionNode(Idle)
        });
    }

    protected virtual List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedChaseNode(transform, player.transform, monsterData.ChaseRange, monsterData.AttackRange),
            new ActionNode(Move)
        };
    }

    protected virtual List<BTNode> BuildAttackSequence()
    {
        return new List<BTNode>
        {
            new IsPreparedAttackNode(transform, player.transform, monsterData.AttackRange, monsterData.AttackCooldown),
            new Selector(BuildAttackSelector())
        };
    }

    protected abstract List<BTNode> BuildAttackSelector();

    protected virtual void Idle() { Debug.Log("Idle"); }
    protected virtual void Move() { Debug.Log("Move"); }
    protected virtual void Attack() { Debug.Log("Attack"); }
}
