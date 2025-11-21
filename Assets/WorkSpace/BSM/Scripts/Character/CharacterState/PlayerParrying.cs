using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParrying : PlayerState
{
    private const float PARRYING_TIME = 0.25f;


    private int parryingHash;
    
    public PlayerParrying(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        int index = playerController.CurrentWeaponType switch
        {
            CharacterWeaponType.SHORT_SWORD => 0,
            CharacterWeaponType.SPEAR => 1,
            _ => 0
        };
        
        parryingHash = animationHashes[playerController.LastMoveKey][index];

        playerController.CanParrying = false;
        playerController.IsParrying = true;
        playerController.ParryingCo = playerController.StartCoroutine(ParryingCoroutine());
    }

    private IEnumerator ParryingCoroutine()
    {
        float elapsedTime = 0;
        playerController.BodyAnimator.Play(parryingHash);
        playerController.WeaponAnimator.Play(parryingHash);
        
        while (elapsedTime < PARRYING_TIME)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        playerController.IsParrying = false;
        playerController.ResetParryingCoolDown(BuffType.PARRYING);
    }
}