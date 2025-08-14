using System.Collections.Generic;

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
    
    //네펜데스의 경우는 움직임이 없기에 오버라이딩하여 상태 넘겨줌
    protected override List<BTNode> BuildChaseSequence()
    {
        return new List<BTNode>
        {
            new AlwaysFailNode()
        };
    }


    protected abstract List<BTNode> BuildSkillPatterns();
    protected abstract List<BTNode> BuildAttackPatterns();
}