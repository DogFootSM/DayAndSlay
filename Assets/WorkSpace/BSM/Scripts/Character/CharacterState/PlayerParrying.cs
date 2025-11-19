using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParrying : PlayerState
{
    public PlayerParrying(PlayerController playerController) : base(playerController){}

    public override void Enter()
    {
        Debug.Log("패링 상태 진입");
    }
    
}
