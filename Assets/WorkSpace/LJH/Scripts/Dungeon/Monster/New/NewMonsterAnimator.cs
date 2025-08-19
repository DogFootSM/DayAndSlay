using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class NewMonsterAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;

    [SerializedDictionary("ActionName", "SpriteLibrary")]
    [SerializeField]
    protected SerializedDictionary<string, SpriteLibraryAsset> spriteDict = new SerializedDictionary<string, SpriteLibraryAsset>();


    private int currentAnimationHash;

    private bool isAction = false;
    public bool IsPlayingAction => isAction;              

    protected Dictionary<Direction, string> moveAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> attackAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> hitAction = new Dictionary<Direction, string>();

    Direction dir = Direction.Down;                       
   
    private Direction attackDir;
    private Direction moveDir;


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        /*
        stateMachine = new MonsterStateMachine(this);
        stateMachine.ChangeState(new MonsterIdleState());
        */

        DictinaryInit();

        /*
        player = GameObject.FindWithTag("Player");
        StartCoroutine(dirCoroutine());
        */
    }

    private void Update()
    {
        if (isAction)
        {
            // 애니메이션 진행도 확인
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // SetDirection() 호출 → 현재는 외부에서 갱신된 dir 사용
            currentAnimationHash = SetAnimationHash(attackAction[dir]);
            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                isAction = false;
            }

            currentAnimationHash = SetAnimationHash(hitAction[dir]);
            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                isAction = false;
                PlayIdle();
            }
        }
    }


    // 이동/행동 벡터 기반으로 외부에서 방향을 지정
    public void SetFacingByDelta(Vector2 delta)
    {
        if (delta.sqrMagnitude < 0.0001f) return; // 거의 0이면 무시
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            dir = (delta.x > 0) ? Direction.Right : Direction.Left;
        else
            dir = (delta.y > 0) ? Direction.Up : Direction.Down;
    }

    int SetAnimationHash(string currentAnimation)
    {
        return Animator.StringToHash("Base Layer." + currentAnimation);
    }

    public void PlayIdle()
    {
        if (isAction) return;

        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.MOVE)];
        animator.Play("MonsterIdle");
    }

    public void PlayMove()
    {
        if (isAction) return;

        // moveDir = SetDirection();
        moveDir = dir; // 외부에서 세팅된 dir 사용
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.MOVE)];
        animator.Play(moveAction[moveDir]);
    }

    public void PlayAttack()
    {
        if (isAction) return;
        
        isAction = true;

        // attackDir = SetDirection();
        attackDir = dir; // 외부에서 세팅된 dir 사용
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(attackAction[attackDir]);
    }

    public void PlayHit()
    {
        if (isAction) return;

        isAction = true;

        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.HIT)];
        // animator.Play(hitAction[SetDirection()]);
        animator.Play(hitAction[dir]); // 외부에서 세팅된 dir 사용
    }

    public void PlayDie()
    {
        Debug.Log("죽음 애니메이션 실행됨");
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.DIE)];
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