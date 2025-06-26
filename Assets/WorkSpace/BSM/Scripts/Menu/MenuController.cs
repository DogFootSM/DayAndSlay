using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private SystemWindowController systemWindowController;
    
    private void Awake()
    {
        continueButton.onClick.AddListener(CloseMenu);
        optionButton.onClick.AddListener(OpenSettingPanel);
    }

    /// <summary>
    /// 메뉴 판넬 종료
    /// </summary>
    private void CloseMenu()
    {
        transform.parent.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 메뉴 판넬 종료 후 설정창 오픈
    /// </summary>
    private void OpenSettingPanel()
    {
        transform.parent.gameObject.SetActive(false);
        systemWindowController.OpenSystemWindow(SystemType.SETTING); 
    }
    
}
