using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheeAnimator : NewMonsterAnimator
{
    protected override void DictionaryInit()
    {
        moveAction.Add(Direction.Left, "MonsterMoveLeft");
        moveAction.Add(Direction.Right, "MonsterMoveRight");
        moveAction.Add(Direction.Up, "MonsterMoveUp");
        moveAction.Add(Direction.Down, "MonsterMoveDown");

        attackAction.Add(Direction.Left, "MonsterAttackLeft");
        attackAction.Add(Direction.Right, "MonsterAttackRight");
        attackAction.Add(Direction.Up, "MonsterAttackUp");
        attackAction.Add(Direction.Down, "MonsterAttackDown");

        hitAction.Add(Direction.Left, "MonsterHitLeft");
        hitAction.Add(Direction.Right, "MonsterHitRight");
        hitAction.Add(Direction.Up, "MonsterHitUp");
        hitAction.Add(Direction.Down, "MonsterHitDown");
        
        skillFirstAction.Add(Direction.Left, "MonsterSkillFirstDown");
        skillFirstAction.Add(Direction.Right, "MonsterSkillFirstDown");
        skillFirstAction.Add(Direction.Up, "MonsterSkillFirstDown");
        skillFirstAction.Add(Direction.Down, "MonsterSkillFirstDown");
        
        skillSecondAction.Add(Direction.Left, "MonsterSkillSecondDown");
        skillSecondAction.Add(Direction.Right, "MonsterSkillSecondDown");
        skillSecondAction.Add(Direction.Up, "MonsterSkillSecondDown");
        skillSecondAction.Add(Direction.Down, "MonsterSkillSecondDown");
        
        skillThirdAction.Add(Direction.Left, "MonsterSkillThirdDown");
        skillThirdAction.Add(Direction.Right, "MonsterSkillThirdDown");
        skillThirdAction.Add(Direction.Up, "MonsterSkillThirdDown");
        skillThirdAction.Add(Direction.Down, "MonsterSkillThirdDown");
        
        skillFourthAction.Add(Direction.Left, "MonsterSkillFourthLeft");
        skillFourthAction.Add(Direction.Right, "MonsterSkillFourthRight");
        skillFourthAction.Add(Direction.Up, "MonsterSkillFourthUp");
        skillFourthAction.Add(Direction.Down, "MonsterSkillFourthLeft");
    }
}
