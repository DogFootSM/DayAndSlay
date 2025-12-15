using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedHelpController : MonoBehaviour
{
    [SerializeField] private Button needHelpButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject needHelpPopup;

    [SerializeField] private Button descTabButton;
    [SerializeField] private Button keyTabButton;
    
    [SerializeField] private List<GameObject> tabViewLists = new List<GameObject>();
    
    private void Awake()
    {
        needHelpButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySfx(SFXSound.UI_BUTTON_CLICK);
            needHelpPopup.SetActive(true);
        });
        
        closeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySfx(SFXSound.UI_BUTTON_CLICK);
            needHelpPopup.SetActive(false);
        });
        
        descTabButton.onClick.AddListener(() => ChangeTabView(0));
        keyTabButton.onClick.AddListener(() => ChangeTabView(1)); 
    }

    private void OnEnable()
    {
        ChangeTabView(0);
    }

    private void ChangeTabView(int viewIndex)
    {
        tabViewLists[0].gameObject.SetActive(viewIndex == 0);
        tabViewLists[1].gameObject.SetActive(viewIndex == 1);
    }
    
}
