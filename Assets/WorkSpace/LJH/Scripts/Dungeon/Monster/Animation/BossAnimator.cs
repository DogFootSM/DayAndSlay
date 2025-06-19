using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class BossAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;

    [SerializedDictionary("ActionName", "SpriteLibrary")]
    [SerializeField]
    protected SerializedDictionary<string, SpriteLibraryAsset> spriteDict = new SerializedDictionary<string, SpriteLibraryAsset>();

    BossMonsterStateMachine stateMachine;

    private int currentAttackHash;

    private bool isAction = false;

    protected Dictionary<Direction, string> moveAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> attackAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> hitAction = new Dictionary<Direction, string>();

    Direction dir;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        stateMachine = new BossMonsterStateMachine(this);

        stateMachine.ChangeState(new BossMonsterIdleState());

        DictinaryInit();

    }

    
    private void Update()
    {
        dir = Direction.Left;

        if (isAction)
        {
            //애니메이션 진행도 확인
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            currentAttackHash = SetAttackHash(attackAction[dir]);

            if (stateInfo.fullPathHash == currentAttackHash && stateInfo.normalizedTime >= 1f)
            {
                isAction = false;
            }
        }
    }
    
    int SetAttackHash(string currentAttack)
    {
        return Animator.StringToHash("Base Layer." + currentAttack);
    }

    public void PlayIdle() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
        animator.Play("MonsterIdle");
    }
    public void PlayMove() 
    {
        if (isAction) return;

        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];

        animator.Play(moveAction[dir]);
    }
    public void PlayAttack() 
    {
        if (isAction) return;

        isAction = true;
        spriteLibrary.spriteLibraryAsset = spriteDict["Attack"];

        animator.Play(attackAction[dir]);

    }
    public void PlayHit() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Hit"];

        animator.Play(hitAction[dir]);
    }
    public void PlayDie() 
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Die"];
        animator.Play("MonsterDie");
    }

    void DictinaryInit()
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
    }
    
}
