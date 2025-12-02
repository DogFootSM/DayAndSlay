using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SystemWindowButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private SystemType systemType;
    [SerializeField] private SystemWindowController windowController;
    [SerializeField] private Animator _tabAnimator;
    [NonSerialized] public bool isActive;
        
    private Button systemWindowButton;
    private SoundManager soundManager => SoundManager.Instance;
    
    private int _tabOpenAnimHash = Animator.StringToHash("TabOpen");
    private int _tabCloseAnimHash = Animator.StringToHash("TabClose");
    
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

        _tabAnimator.Rebind();
        _tabAnimator.Play(_tabOpenAnimHash);
    }

    public void CloseTab()
    {
        isActive = false;
        
        _tabAnimator.Play(_tabCloseAnimHash);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlaySfx(SFXSound.INVENTORY_TAB_HOVER);
    }
}
