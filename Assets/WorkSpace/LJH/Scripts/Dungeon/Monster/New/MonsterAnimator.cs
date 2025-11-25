using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class MonsterAnimator : MonoBehaviour
{
    protected Animator animator;
    protected SpriteLibrary spriteLibrary;

    [SerializedDictionary("ActionName", "SpriteLibrary")]
    [SerializeField]
    protected SerializedDictionary<string, SpriteLibraryAsset> spriteDict = new SerializedDictionary<string, SpriteLibraryAsset>();


    protected int currentAnimationHash;

    public bool isAction;
    public bool IsPlayingAction => isAction;              

    protected Dictionary<Direction, string> moveAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> attackAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> hitAction = new Dictionary<Direction, string>();
    
    //보스 전용
    protected Dictionary<Direction, string> skillFirstAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> skillSecondAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> skillThirdAction = new Dictionary<Direction, string>();
    protected Dictionary<Direction, string> skillFourthAction = new Dictionary<Direction, string>();

    protected Direction dir = Direction.Down;                       
   
    protected Direction attackDir;
    protected Direction moveDir;

    public Direction GetCurrentDir() => dir;


    protected void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        DictionaryInit();
    }

    public void SetIsAction(bool value)
    {
        isAction = value;
    }

    protected void Update()
    {
        
        if (isAction)
        {
            // 애니메이션 진행도 확인
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            currentAnimationHash = SetAnimationHash(attackAction[dir]);
            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                PlayIdle();
                isAction = false;
            }

            currentAnimationHash = SetAnimationHash(hitAction[dir]);
            if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
            {
                PlayIdle();
                isAction = false;
            }
            
            if (gameObject.CompareTag("Boss"))
            {
                currentAnimationHash = SetAnimationHash(skillFirstAction[dir]);
                if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
                {
                    isAction = false;
                }

                currentAnimationHash = SetAnimationHash(skillSecondAction[dir]);
                if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
                {
                    isAction = false;
                }

                currentAnimationHash = SetAnimationHash(skillThirdAction[dir]);
                if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
                {
                    isAction = false;
                }

                currentAnimationHash = SetAnimationHash(skillFourthAction[dir]);
                if (stateInfo.fullPathHash == currentAnimationHash && stateInfo.normalizedTime >= 1f)
                {
                    isAction = false;
                }
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

    public virtual void PlayIdle()
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

    public IEnumerator PlayCounterCoroutine()
    {
        animator.speed = 0f;
        
        yield return new WaitForSeconds(0.75f);


        isAction = false;

        PlayHit();
        animator.speed = 1f;
    }


    public void PlayHit()
    {
        if (isAction) return;

        isAction = true;

        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.HIT)];
        // animator.Play(hitAction[SetDirection()]);
        animator.Play(hitAction[dir]); // 외부에서 세팅된 dir 사용
    }

    /// <summary>
    /// 애니메이션을 duration 동안 멈춰줌
    /// </summary>
    /// <param name="duration"></param>
    public void PlayStun(float duration)
    {
        isAction = true;

        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.HIT)];
        // animator.Play(hitAction[SetDirection()]);
        animator.Play(hitAction[dir]); 
        
        StartCoroutine(StunCoroutine(duration));
    }

    
    private IEnumerator StunCoroutine(float duration)
    {
        animator.speed = 0f;
        
        yield return new WaitForSeconds(duration);
        isAction = false;
        PlayIdle();
        animator.speed = 1f;
    }

    public void PlayDie()
    {
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.DIE)];
        animator.Play("MonsterDie");
    }

    /// <summary>
    /// 보스몬스터 스킬1
    /// </summary>
    public void PlaySkillFirst()
    {
        if (isAction) return;
        
        isAction = true;
        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillFirstAction[attackDir]);
    }
    
    /// <summary>
    /// 보스몬스터 스킬2
    /// </summary>
    public void PlaySkillSecond()
    {
        if (isAction) return;
        
        isAction = true;

        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillSecondAction[attackDir]);
    }
    
    /// <summary>
    /// 보스몬스터 스킬3
    /// </summary>
    public void PlaySkillThird()
    {
        if (isAction) return;
        
        isAction = true;

        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillThirdAction[attackDir]);
    }
    
    /// <summary>
    /// 보스몬스터 스킬4
    /// </summary>
    public void PlaySkillFourth()
    {
        if (isAction) return;
        
        isAction = true;

        attackDir = dir;
        spriteLibrary.spriteLibraryAsset = spriteDict[nameof(AnimType.ATTACK)];
        animator.Play(skillFourthAction[attackDir]);
    }

    protected virtual void DictionaryInit()
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
        
        if(gameObject.CompareTag("Boss"))
        {
            skillFirstAction.Add(Direction.Left, "MonsterSkillFirstLeft");
            skillFirstAction.Add(Direction.Right, "MonsterSkillFirstRight");
            skillFirstAction.Add(Direction.Up, "MonsterSkillFirstUp");
            skillFirstAction.Add(Direction.Down, "MonsterSkillFirstDown");

            skillSecondAction.Add(Direction.Left, "MonsterSkillSecondLeft");
            skillSecondAction.Add(Direction.Right, "MonsterSkillSecondRight");
            skillSecondAction.Add(Direction.Up, "MonsterSkillSecondUp");
            skillSecondAction.Add(Direction.Down, "MonsterSkillSecondDown");

            skillThirdAction.Add(Direction.Left, "MonsterSkillThirdLeft");
            skillThirdAction.Add(Direction.Right, "MonsterSkillThirdRight");
            skillThirdAction.Add(Direction.Up, "MonsterSkillThirdUp");
            skillThirdAction.Add(Direction.Down, "MonsterSkillThirdDown");

            skillFourthAction.Add(Direction.Left, "MonsterSkillFourthLeft");
            skillFourthAction.Add(Direction.Right, "MonsterSkillFourthRight");
            skillFourthAction.Add(Direction.Up, "MonsterSkillFourthUp");
            skillFourthAction.Add(Direction.Down, "MonsterSkillFourthDown");
        }
    }
}