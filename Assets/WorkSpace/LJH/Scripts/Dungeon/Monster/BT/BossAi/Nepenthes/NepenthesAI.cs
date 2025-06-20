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

        // 거리 체크 + 원거리 공격
        BTNode rangeCombo = new Sequence(new List<BTNode>()
    {
        rangeCheckNode,
        rangeAttackSequence
    });

        nodes.Add(rangeCombo);         // 조건 만족 시 원거리
        nodes.Add(meleeAttackSequence); // 실패 시 근거리

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

        rangeCheckNode = new IsAttackRangeNode(transform, player.transform, 5f);
    }
}
