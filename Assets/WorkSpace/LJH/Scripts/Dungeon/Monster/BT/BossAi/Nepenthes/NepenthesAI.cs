using System.Collections.Generic;
using UnityEngine;

public abstract class NepenthesAI : BossMonsterAI
{
    protected override List<BTNode> BuildSkillSelector()
    {
        return BuildSkillPatterns();
    }

    protected override List<BTNode> BuildAttackSelector()
    {
        return BuildAttackPatterns();
    }

    protected override void Move()
    {
        Debug.Log(name + " -> Move");
    }

    protected abstract List<BTNode> BuildSkillPatterns();
    protected abstract List<BTNode> BuildAttackPatterns();
}