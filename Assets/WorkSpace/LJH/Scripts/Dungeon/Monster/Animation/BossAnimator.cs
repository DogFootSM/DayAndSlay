using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(Animator))]
public class BossAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteLibrary spriteLibrary;

    [SerializeField]
    private SerializedDictionary<string, SpriteLibraryAsset> spriteDict =
        new SerializedDictionary<string, SpriteLibraryAsset>();

    public BossMonsterStateMachine stateMachine;

    private int currentAttackHash;
    private bool isAction = false;

    public bool IsPlayingAction
    {
        get { return isAction; }
    }

    private Dictionary<Direction, string> moveAction = new Dictionary<Direction, string>();
    private Dictionary<Direction, string> attackAction = new Dictionary<Direction, string>();
    private Dictionary<Direction, string> skillFirstAction = new Dictionary<Direction, string>();
    private Dictionary<Direction, string> skillSecondAction = new Dictionary<Direction, string>();
    private Dictionary<Direction, string> hitAction = new Dictionary<Direction, string>();

    private Direction dir;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteLibrary = GetComponent<SpriteLibrary>();

        stateMachine = new BossMonsterStateMachine(this);
        stateMachine.ChangeState(new BossMonsterIdleState());

        InitDictionaries();

        dir = Direction.Left;
    }

    private void Update()
    {
        if (isAction)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            int attackHash = Animator.StringToHash("Base Layer." + attackAction[dir]);
            int skillHash = Animator.StringToHash("Base Layer.BossSkillFirst");

            if ((stateInfo.fullPathHash == attackHash || stateInfo.fullPathHash == skillHash) 
                && stateInfo.normalizedTime >= 1f)
            {
                isAction = false;
            }
        }
    }

    public void PlayIdle()
    {
        if(isAction) return;
        
        spriteLibrary.spriteLibraryAsset = spriteDict["Move"];
        animator.Play("BossIdle");
        isAction = false;
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

    public void PlaySkillFirst()
    {
        if (isAction) return;
        
        isAction = true;
        spriteLibrary.spriteLibraryAsset = spriteDict["Attack"];
        animator.Play(skillFirstAction[dir]);
    }
    
    public void PlaySkillSecond()
    {
        if (isAction) return;
        
        isAction = true;
        spriteLibrary.spriteLibraryAsset = spriteDict["Attack"];
        animator.Play(skillSecondAction[dir]);
    }
    

    public void PlayDie()
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Die"];
        animator.Play("BossDie");
        isAction = false;
    }

    public void PlayHit()
    {
        spriteLibrary.spriteLibraryAsset = spriteDict["Hit"];
        animator.Play(hitAction[dir]);
    }

    private void InitDictionaries()
    {
        moveAction.Add(Direction.Left, "BossMoveLeft");
        moveAction.Add(Direction.Right, "BossMoveRight");
        moveAction.Add(Direction.Up, "BossMoveUp");
        moveAction.Add(Direction.Down, "BossMoveDown");

        attackAction.Add(Direction.Left, "BossAttackLeft");
        attackAction.Add(Direction.Right, "BossAttackRight");
        attackAction.Add(Direction.Up, "BossAttackUp");
        attackAction.Add(Direction.Down, "BossAttackDown");
        
        skillFirstAction.Add(Direction.Left, "BossSkillFirstLeft");
        skillFirstAction.Add(Direction.Right, "BossSkillFirstRight");
        skillFirstAction.Add(Direction.Up, "BossSkillFirstUp");
        skillFirstAction.Add(Direction.Down, "BossSkillFirstDown");
        
        skillSecondAction.Add(Direction.Left, "BossSkillSecondLeft");
        skillSecondAction.Add(Direction.Right, "BossSkillSecondRight");
        skillSecondAction.Add(Direction.Up, "BossSkillSecondUp");
        skillSecondAction.Add(Direction.Down, "BossSkillSecondDown");

        hitAction.Add(Direction.Left, "BossHitLeft");
        hitAction.Add(Direction.Right, "BossHitRight");
        hitAction.Add(Direction.Up, "BossHitUp");
        hitAction.Add(Direction.Down, "BossHitDown");
    }
}
