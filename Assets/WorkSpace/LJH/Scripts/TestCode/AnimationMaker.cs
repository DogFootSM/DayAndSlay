using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AnimationMaker : MonoBehaviour
{
    private Dictionary<Direction, string> dir;
    [SerializeField] SpriteResolver resolver;
    
    [SerializeField] AnimationClip clipOne;

    void Start()
    {
        dir[Direction.Up] = "UPMOVE";
        dir[Direction.Right] = "SIDEMOVE";
        dir[Direction.Down] = "DOWNMOVE";
        
        
        resolver = new SpriteResolver();
        resolver.SetCategoryAndLabel(dir[Direction.Up], "0");
        
    }
}
