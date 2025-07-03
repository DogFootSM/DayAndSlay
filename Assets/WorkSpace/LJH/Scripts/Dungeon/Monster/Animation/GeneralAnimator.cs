using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class GeneralAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;

    [SerializedDictionary("ActionName", "SpriteLibrary")]
    [SerializeField]
    protected SerializedDictionary<string, SpriteLibraryAsset> spriteDict = new SerializedDictionary<string, SpriteLibraryAsset>();

    MonsterStateMachine stateMachine;

    private int currentAnimationHash;

    private bool isAction = false;

    protected Dictionary<Direction, string> moveAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> attackAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> hitAction = new Dictionary<Direction, string>();

    Direction dir;
    GameObject player;

    private Direction attackDir;
    private Direction moveDir;

    float term = 0.1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        stateMachine = new MonsterStateMachine(this);

        stateMachine.ChangeState(new MonsterIdleState());

        DictinaryInit();
        player = GameObject.FindWithTag("Player");

        StartCoroutine(dirCoroutine());
    }


    private void Update()
    {
        if (isAction)
        {
            //애니메이션 진행도 확인
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            currentAnimationHash = SetAnimationHash(attackAction[SetDirection()]);

            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                Debug.Log($"공격 끝나고 isAction");
                isAction = false;
            }

            currentAnimationHash = SetAnimationHash(hitAction[SetDirection()]);

            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                Debug.Log($"피격 끝나고 isAction");
                isAction = false;
            }
        }
    }

    IEnumerator dirCoroutine()
    {
        //현재 이건 의미 없는 상태
        //because : 애니메이션의 방향을 지정해서 그 애니메이션을 실행 시키는 로직이라
        //실행 시킨 이후에 방향을 바꿔주는건 의미 없음
        while (true)
        {
            yield return new WaitForSeconds(term);
            SetDirection();
        }
    }

    Direction SetDirection()
    {
        //방향 변경을 꺾을때마다 해줘야함
        //현재 상황은 처음 애니메이션 시작할 때 방향을 정해줘서
        //계속 한 방향으로 이동하는 상태

        Vector2 diff = player.transform.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            dir = (diff.x > 0) ? Direction.Right : Direction.Left;
        }
        else
        {
            dir = (diff.y > 0) ? Direction.Up : Direction.Down;
        }

        return dir;
    }

    int SetAnimationHash(string currentAnimation)
    {
        return Animator.StringToHash("Base Layer." + currentAnimation);
    }

    public void PlayIdle()
    {
        if (isAction) return;

        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
        animator.Play("MonsterIdle");

    }
    public void PlayMove()
    {
        if (isAction) return;

        moveDir = SetDirection();
        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
        animator.Play(moveAction[moveDir]);
    }
    public void PlayAttack()
    {

        if (isAction) return;

        isAction = true;

        attackDir = SetDirection();
        spriteLibrary.spriteLibraryAsset = spriteDict["Attack"];
        animator.Play(attackAction[attackDir]);

    }
    public void PlayHit()
    {
        if (isAction) return;

        isAction = true;

        spriteLibrary.spriteLibraryAsset = spriteDict["Hit"];
        animator.Play(hitAction[Direction.Left]);
    }
    public void PlayDie()
    {
        Debug.Log("죽음 애니메이션 실행됨");
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
