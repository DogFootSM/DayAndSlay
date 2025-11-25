using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    public static PlayerRoot PlayerRootInstance;

    private void Awake()
    {
        if (PlayerRootInstance == null)
        {
            PlayerRootInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
