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

        stateMachine.ChangeState(new MonsterMoveState());

    }

    private void Update()
    {
        Change();
    }

    private void Change()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(new MonsterAttackState());
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            stateMachine.ChangeState(new MonsterMoveState());
        }
    }

    public void PlayIdle() 
    {
        if(spriteLibrary == null)
        {
            Debug.Log("스프라이트 라이브러리가 널임");
        }
        if (spriteLibrary.spriteLibraryAsset == null)
        {
            Debug.Log("스프라이트 라이브러리의 애셋이 널임");
        }
        if (spriteDict["Move"] == null)
        {
            Debug.Log("스프라이트 딕셔너리또는 무브가가 널임");
        }
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
