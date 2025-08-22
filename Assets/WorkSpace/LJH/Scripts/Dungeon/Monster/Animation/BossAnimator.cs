using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class BossAnimator : NewMonsterAnimator
{
    private Dictionary<Direction, string> skillFirstAction = new Dictionary<Direction, string>();
    private Dictionary<Direction, string> skillSecondAction = new Dictionary<Direction, string>();

    public override void PlayIdle()
    {
        if (isAction) return;

        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.MOVE)];
        animator.Play("BossIdle");
    }
    
    public void PlaySkillFirst()
    {
        if (isAction) return;
        
        isAction = true;

        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillFirstAction[dir]);
    }
    
    public void PlaySkillSecond()
    {
        if (isAction) return;
        
        isAction = true;

        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillSecondAction[dir]);
    }
    

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
        
        skillFirstAction.Add(Direction.Left, "MonsterSkillFirstLeft");
        skillFirstAction.Add(Direction.Right, "MonsterSkillFirstRight");
        skillFirstAction.Add(Direction.Up, "MonsterSkillFirstUp");
        skillFirstAction.Add(Direction.Down, "MonsterSkillFirstDown");
        
        skillSecondAction.Add(Direction.Left, "MonsterSkillSecondLeft");
        skillSecondAction.Add(Direction.Right, "MonsterSkillSecondRight");
        skillSecondAction.Add(Direction.Up, "MonsterSkillSecondUp");
        skillSecondAction.Add(Direction.Down, "MonsterSkillSecondDown");
    }
}
