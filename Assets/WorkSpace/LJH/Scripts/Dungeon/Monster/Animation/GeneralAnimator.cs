using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class GeneralAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;
    [SerializeField] protected List<SpriteLibraryAsset> spriteLibraries;
    protected Dictionary<string, SpriteLibraryAsset> spriteDict = new Dictionary<string, SpriteLibraryAsset>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        spriteDict.Add("Attack", spriteLibraries[0]);
        spriteDict.Add("Die", spriteLibraries[1]);
        spriteDict.Add("Hit", spriteLibraries[2]);
        spriteDict.Add("Move", spriteLibraries[3]);

    }

    public void PlayIdle() 
    {
        animator.Play("MonsterIdle");
    }
    public void PlayMove() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
        animator.Play("MonsterMoveLeft");
        //animator.Play("MonsterMoveRight");
        //animator.Play("MonsterMoveUp");
        //animator.Play("MonsterMoveDown");
    }
    public void PlayAttack() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Attack"];
        animator.Play("MonsterAttackLeft");
        //animator.Play("MonsterAttackRight");
        //animator.Play("MonsterAttackUp");
        //animator.Play("MonsterAttackDown");
    }
    public void PlayHit() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Hit"];
        animator.Play("MonsterHitLeft");
        animator.Play("MonsterHitRight");
        animator.Play("MonsterHitUp");
        animator.Play("MonsterHitDown");
    }
    public void PlayDie() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Die"];
        animator.Play("MonsterDie");
    }

}
