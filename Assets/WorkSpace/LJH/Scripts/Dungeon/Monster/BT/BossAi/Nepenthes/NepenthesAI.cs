using System.Collections.Generic;
using UnityEngine;

public class NepenthesAI : BossMonsterAI
{
    protected override List<BTNode> BuildAttackSelector()
    {
        return new List<BTNode>
    {
        new Sequence(new List<BTNode>
        {
            //원거리 공격용 시퀀스
            
            new IsAttackRangeNode(transform, player.transform, 5f),
            //new SetAnimatorStateNode(animator.stateMachine, new MonsterAttackState()),
            //new ShootProjectileNode(transform, player.transform)
        }),
        new Sequence(new List<BTNode>
        {
            //근거리 공격용 시퀀스

            //new PlayAnimationNode(animator, "Bite"),
            //new DealDamageNode(player)
        })
        };
    }
}
