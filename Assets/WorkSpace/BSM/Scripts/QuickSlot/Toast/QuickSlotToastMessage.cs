using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotToastMessage : MonoBehaviour
{
    [SerializeField] private Animator toastAnimator;

    private int toastHash;

    private void Awake()
    {
        toastHash = Animator.StringToHash("ShowToast");
    }

    public void ShowToast()
    {
        toastAnimator.SetTrigger(toastHash);
    }
    
}
