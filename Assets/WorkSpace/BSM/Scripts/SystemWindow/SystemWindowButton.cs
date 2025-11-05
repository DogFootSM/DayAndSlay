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
    
    private void Awake() => Init();

    private void Init()
    {
        systemWindowButton = GetComponent<Button>();
        
        systemWindowButton.onClick.AddListener(() => windowController.OpenSystemWindow(systemType));
    }
     
    
}
