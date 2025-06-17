using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PlayerState
{
    private QuickSlotType quickSlotType;
    private Coroutine afterDelayCo;
    private float afterDelay;
    
    public PlayerSkill(PlayerController playerController) : base(playerController) {}
    
    public override void Enter()
    {
        quickSlotType = skillKey switch
        {
            KeyCode.Q => QuickSlotType.Q,
            KeyCode.W => QuickSlotType.W,
            KeyCode.E => QuickSlotType.E,
            KeyCode.R => QuickSlotType.R,
            KeyCode.A => QuickSlotType.A,
            KeyCode.S => QuickSlotType.S,
            KeyCode.D => QuickSlotType.D,
            KeyCode.F => QuickSlotType.F,
            _ => QuickSlotType.NONE
        };

        UseSkill(); 
    }

    public override void Exit()
    {
        Debug.Log("스킬 상태 종료");
        if (afterDelayCo != null)
        {
            playerController.StopCoroutine(afterDelayCo);
            afterDelayCo = null;
        }
        
    }

    /// <summary>
    /// 스킬 사용
    /// </summary>
    private void UseSkill()
    {
        afterDelay = playerController.SkillSlotInvoker.SkillInvoke(quickSlotType);

        if (afterDelayCo == null)
        {
            afterDelayCo = playerController.StartCoroutine(AfterDelayRoutine());
        }
    }
    
    /// <summary>
    /// 스킬 후딜레이 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterDelayRoutine()
    { 
        yield return playerController.WaitCache.GetWait(afterDelay); 
        playerController.ChangeState(CharacterStateType.IDLE);
    }
    

}
