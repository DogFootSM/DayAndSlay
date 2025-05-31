using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;

public class PlayerAttack : PlayerState
{
    private int attackHash; 

    private Coroutine attackCo;

    public PlayerAttack(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        attackHash = playerController.LastMoveKey switch
        {
            Direction.Up => upAttackHash,
            Direction.Down => downAttackHash,
            Direction.Right => leftAttackHash,
            Direction.Left => rightAttackHash,
            _ => throw new ArgumentNullException("잘못된 입력")
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
        playerController.CurWeapon.NormalAttack();
        playerController.BodyAnimator.Play(attackHash);
        playerController.WeaponAnimator.Play(attackHash);
        yield return playerController.WaitCache.GetWait(attackSpeed);

        playerController.ChangeState(CharacterStateType.IDLE);
    }
}