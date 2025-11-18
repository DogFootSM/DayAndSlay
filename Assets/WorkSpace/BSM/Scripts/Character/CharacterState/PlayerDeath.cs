using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : PlayerState
{
    public PlayerDeath(PlayerController playerController) : base(playerController) {}

    public override void Enter()
    {
        playerController.PlayerDeath();
    }

    public override void Update()
    {
        // !IsDead -> StateChange(Idle); 
    }
    
}
