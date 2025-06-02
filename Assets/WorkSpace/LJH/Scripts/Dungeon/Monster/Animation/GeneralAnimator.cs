using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class GeneralAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;

    [SerializedDictionary("ActionName", "SpriteLibrary")]
    [SerializeField]
    protected SerializedDictionary<string, SpriteLibraryAsset> spriteDict = new SerializedDictionary<string, SpriteLibraryAsset>();

    MonsterStateMachine stateMachine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        stateMachine = new MonsterStateMachine(this);

        stateMachine.ChangeState(new MonsterIdleState());

    }

    public void PlayIdle() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
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
