using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerStateMachine
{
    protected static KeyCode skillInputKey;
    protected float attackSpeed => playerController.PlayerModel.PlayerStats.attackSpeed;
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
    protected int downAttackHash = Animator.StringToHash("DownAttack");
    protected int upAttackHash = Animator.StringToHash("UpAttack");
    protected int leftAttackHash = Animator.StringToHash("LeftAttack");
    protected int rightAttackHash = Animator.StringToHash("RightAttack"); 
    
    //Walk BlendTree
    protected int walkBlendTreeHash = Animator.StringToHash("WalkBlend");
    
    //BlendTree X,Y 값
    protected int walkPosXHash = Animator.StringToHash("WalkPosX");
    protected int walkPosYHash = Animator.StringToHash("WalkPosY");
    
    //TODO: 장착 무기 타입에 따라 공격 해시값 변경 필요
    
    //protected int swordUpAttackAnimHash = Animator.StringToHash("CharacterUpAttackAnim")
    //upAttackAnimHash = swordUpAttackAnimHash;
    
    
    public PlayerState(PlayerController playerController)
    {
        this.playerController = playerController; 
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
