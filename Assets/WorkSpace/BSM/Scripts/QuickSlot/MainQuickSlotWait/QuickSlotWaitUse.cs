using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotWaitUse : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RectTransform rectTransform;
    
    private int useAnimHash;

    private void Awake()
    {
        useAnimHash = Animator.StringToHash("WaitUse");
    }

    public void PlayAnimation()
    {
        animator.SetBool(useAnimHash, true);
    }

    public void StopAnimation()
    {
        animator.SetBool(useAnimHash, false);
    }

    public void UpdateRectTransform()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
