using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : PlayerState
{ 
    private Coroutine afterDelayCo;
    private float afterDelay;
    
    public PlayerSkill(PlayerController playerController) : base(playerController) {}
    
    public override void Enter()
    {  
        ExecuteSkillFromSlot(); 
    }

    public override void Exit()
    {
        if (afterDelayCo != null)
        {
            playerController.StopCoroutine(afterDelayCo);
            afterDelayCo = null;
        } 
    }

    /// <summary>
    /// 스킬 사용
    /// </summary>
    private void ExecuteSkillFromSlot()
    {
        afterDelay = playerController.SkillSlotInvoker.InvokeSkillFromSlot(keyToQuickSlotMap[skillInputKey], playerController.CurrentWeaponType);

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
        yield return WaitCache.GetWait(afterDelay); 
        playerController.ChangeState(CharacterStateType.IDLE);
    }
    

}
