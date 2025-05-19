using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;

public class PlayerAttack : PlayerState
{
    private int attackHash;
    private StringBuilder sb;

    private Coroutine attackCo; 
    public PlayerAttack(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        sb = new StringBuilder(playerController.LastKey);

        //키 동시 여러개 입력 방지
        if (sb.Length > 1)
        {
            sb.Remove(1, sb.Length - 1);
            playerController.LastKey = sb.ToString();
        }

        attackHash = playerController.LastKey.ToLower() switch
        {
            UpDir => upAttackHash,
            DownDir => downAttackHash,
            LeftDir => leftAttackHash,
            RightDir => rightAttackHash,
            _ => attackHash
        };

        attackCo = playerController.StartCoroutine(AttackExitRoutine());
    }

    public override void Exit()
    {
        if (attackCo != null)
        {
            playerController.StopCoroutine(attackCo);
            attackCo = null;
        }
    }
    
    /// <summary>
    /// 공격 종료 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackExitRoutine()
    {
        playerController.BodyAnimator.Play(attackHash);
        playerController.WeaponAnimator.Play(attackHash); 
        yield return playerController.WaitCache.GetWait(attackSpeed);
    
        playerController.ChangeState(CharacterStateType.IDLE);
    }
    

}
