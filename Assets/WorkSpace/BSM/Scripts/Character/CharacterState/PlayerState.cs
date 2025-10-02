using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected static KeyCode skillInputKey;
    protected float attackSpeed => playerController.PlayerModel.AttackSpeed;
    protected float moveSpeed => playerController.PlayerModel.MoveSpeed;
    
    private KeyCode[] skillInputKeys = new KeyCode[]
    {
        KeyCode.Q,
        KeyCode.W,
        KeyCode.E,
        KeyCode.R,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.F,
    };

    protected Dictionary<KeyCode, QuickSlotType> keyToQuickSlotMap = new Dictionary<KeyCode, QuickSlotType>()
    {
        { KeyCode.Q, QuickSlotType.Q},
        { KeyCode.W, QuickSlotType.W},
        { KeyCode.E, QuickSlotType.E},
        { KeyCode.R, QuickSlotType.R},
        { KeyCode.A, QuickSlotType.A},
        { KeyCode.S, QuickSlotType.S},
        { KeyCode.D, QuickSlotType.D},
        { KeyCode.F, QuickSlotType.F}, 
    };
    
    protected PlayerController playerController;
    
    //Body Idle Hash
    protected int upIdleHash = Animator.StringToHash("UpIdle");
    protected int downIdleHash = Animator.StringToHash("DownIdle");
    protected int leftIdleHash = Animator.StringToHash("LeftIdle");
    protected int rightIdleHash = Animator.StringToHash("RightIdle");
    
    //Body Attack Hash
    protected static Dictionary<Direction, List<int>> animationHashes = new Dictionary<Direction, List<int>>();

    //Body 애니메이션 사용 여부 체크
    protected static bool[][] randHashIndexCheck = new bool[4][];

    
    //Walk BlendTree
    protected int walkBlendTreeHash = Animator.StringToHash("WalkBlend");
    
    //BlendTree X,Y 값
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
     
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController; 
         
        if (!animationHashes.ContainsKey(Direction.Up) &&
            !animationHashes.ContainsKey(Direction.Down) &&
            !animationHashes.ContainsKey(Direction.Left) &&
            !animationHashes.ContainsKey(Direction.Right))
        {
            animationHashes.Add(Direction.Up, new List<int>());
            animationHashes.Add(Direction.Down, new List<int>());
            animationHashes.Add(Direction.Left, new List<int>());
            animationHashes.Add(Direction.Right, new List<int>());

            AddAnimationHashes(Direction.Up, "UpAttack");
            AddAnimationHashes(Direction.Down, "DownAttack");
            AddAnimationHashes(Direction.Left, "RightAttack");
            AddAnimationHashes(Direction.Right, "LeftAttack");
            
            randHashIndexCheck[0] = new bool[3] { false, false, false};
            randHashIndexCheck[1] = new bool[3] { false, false, false};
            randHashIndexCheck[2] = new bool[3] { false, false, false};
            randHashIndexCheck[3] = new bool[3] { false, false, false};
        } 
    }

    /// <summary>
    /// 바라보는 방향에 따른 애니메이션 저장
    /// </summary>
    /// <param name="key">바라보고 있는 방향</param>
    /// <param name="toHash">캐싱할 애니메이션 이름</param>
    private void AddAnimationHashes(Direction key, string toHash)
    {
        for (int i = 0; i < 3; i++)
        {
            animationHashes[key].Add(Animator.StringToHash($"{toHash}{i + 1}"));
        } 
    }
    
    
    /// <summary>
    /// 스킬 키 입력 감지
    /// </summary>
    protected void CheckSkillKeyInput()
    {
        foreach (KeyCode keyCode in skillInputKeys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                skillInputKey = keyCode;

                if (QuickSlotData.IsSlotAssigned(keyToQuickSlotMap[skillInputKey],playerController.CurrentWeaponType))
                {
                    playerController.ChangeState(CharacterStateType.SKILL);
                } 
            } 
        } 
    }
}
