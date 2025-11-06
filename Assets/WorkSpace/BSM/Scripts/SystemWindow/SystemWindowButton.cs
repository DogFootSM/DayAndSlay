using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemWindowButton : MonoBehaviour
{
    [SerializeField] private SystemType systemType;
    [SerializeField] private SystemWindowController windowController;
    [SerializeField] private Animator _tabAnimator;
    [NonSerialized] public bool isActive;
        
    private Button systemWindowButton;
    private int _tabOpenAnimHash = Animator.StringToHash("TabOpen");
    private int _tabCloseAnimHash = Animator.StringToHash("TabClose");
    private int _tablCloseTrigger = Animator.StringToHash("TabClose");
    
    private void Awake() => Init();

    private void Init()
    {
        systemWindowButton = GetComponent<Button>();
        //_tabAnimator.keepAnimatorStateOnDisable = true;
        //_tabAnimator.writeDefaultValuesOnDisable = true;
        systemWindowButton.onClick.AddListener(() => windowController.OpenSystemWindow(systemType));
    }

    public void SwitchTab(bool isOpen)
    {
        isActive = isOpen;
        
        int playAnim = isOpen ? _tabOpenAnimHash : _tabCloseAnimHash;
        
        _tabAnimator.Play(playAnim);
    }

    public void CloseTab()
    {
        isActive = false;
        _tabAnimator.SetTrigger(_tablCloseTrigger);
    }
    
}
