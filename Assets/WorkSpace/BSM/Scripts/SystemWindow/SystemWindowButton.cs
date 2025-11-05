using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemWindowButton : MonoBehaviour
{
    [SerializeField] private SystemType systemType;
    [SerializeField] private SystemWindowController windowController;
    
    private Button systemWindowButton;
    private Animator _tabAnimator;
    
    private int _tabOpenAnimHash = Animator.StringToHash("TabOpen");
    private int _tabCloseAnimHash = Animator.StringToHash("TabClose");
    
    private void Awake() => Init();

    private void Init()
    {
        systemWindowButton = GetComponent<Button>();
        _tabAnimator = GetComponent<Animator>();
        
        systemWindowButton.onClick.AddListener(() => windowController.OpenSystemWindow(systemType));
    }

    public void SwitchTab(bool isOpen)
    {
        int playAnim = isOpen ? _tabOpenAnimHash : _tabCloseAnimHash;
        
        _tabAnimator.Play(playAnim);
    } 
}
