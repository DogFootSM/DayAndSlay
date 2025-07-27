using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malus : NepenthesAI
{
    protected override List<BTNode> SelectorMethod()
    {
        return new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                //플레이어가 아예 사정거리 밖일 경우 특수 공격 시퀀스 / 사거리는 예시용
                new IsAttackRangeNode(transform, player.transform, 10f)
            
            
            }),
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
