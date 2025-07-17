using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotToastMessage : MonoBehaviour
{
    [SerializeField] private Animator toastAnimator;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Image toastImage;
    
    private int toastHash;

    private void Awake()
    {
        toastHash = Animator.StringToHash("ShowToast");
    }

    private void OnDisable()
    {
        alertText.color = new Color(alertText.color.r, alertText.color.g, alertText.color.b, 0f);
        toastImage.color = new Color(toastImage.color.r, toastImage.color.g, toastImage.color.b, 0f);
    }

    public void ShowToast()
    {
        toastAnimator.SetTrigger(toastHash);
    }
    
}
