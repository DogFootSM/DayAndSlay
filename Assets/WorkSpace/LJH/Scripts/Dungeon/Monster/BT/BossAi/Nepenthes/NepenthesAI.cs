using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NepenthesAI : BossMonsterAI
{
    private BTNode rangeAttackSequence;
    private BTNode meleeAttackSequence;
    private BTNode rangeCheckNode;
    public override List<BTNode> AttackSelector()
    {
        List<BTNode> nodes = new List<BTNode>();
        nodes.Add(rangeAttackSequence);
        nodes.Add(meleeAttackSequence);

        return nodes;
    }

    public List<BTNode> RangeAttackSequence()
    {
        List<BTNode> nodes = new List<BTNode>();


        return nodes;
    }

    public List<BTNode> MeleeAttackSequence()
    {
        List<BTNode> nodes = new List<BTNode>();


        return nodes;
    }

    private void Start()
    {
        rangeAttackSequence = new Sequence(RangeAttackSequence());
        meleeAttackSequence = new Sequence(MeleeAttackSequence());
    }
}
