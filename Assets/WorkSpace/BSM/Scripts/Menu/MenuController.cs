using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private SystemWindowController systemWindowController;
    
    private GameManager gameManager => GameManager.Instance;
    
    private void Awake()
    {
        continueButton.onClick.AddListener(CloseMenu);
        optionButton.onClick.AddListener(OpenSettingPanel);
        quitButton.onClick.AddListener(OnClickQuit);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    /// <summary>
    /// 메뉴 판넬 종료
    /// </summary>
    private void CloseMenu()
    {
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 메뉴 판넬 종료 후 설정창 오픈
    /// </summary>
    private void OpenSettingPanel()
    {
        gameObject.SetActive(false);
        systemWindowController.OpenSystemWindow(SystemType.SETTING); 
    }

    /// <summary>
    /// 게임 종료 버튼 클릭하여 게임 종료 로직 호출
    /// </summary>
    private void OnClickQuit()
    {
        gameManager.MainSceneConfirmQuit();
    }

    private void MainMenu()
    {
        gameManager.CheckMainMenu();
    }
}
