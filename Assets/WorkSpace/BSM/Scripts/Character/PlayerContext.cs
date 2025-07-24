using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    private PlayerController playerController;

    public void Setup(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public PlayerController GetPlayerController()
    {
        return this.playerController;
    }
    
}
