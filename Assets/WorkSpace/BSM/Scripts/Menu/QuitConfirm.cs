using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitConfirm : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    
    private GameManager gameManager => GameManager.Instance;
    private bool isExit;
    
    private void Awake()
    { 
        confirmButton.onClick.AddListener(ConfirmQuit);
        cancelButton.onClick.AddListener(CloseMenuPanel);
    }

    /// <summary>
    /// 데이터 저장 안내 얼럿 판넬 종료
    /// </summary>
    private void CloseMenuPanel()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임 종료 진행
    /// </summary>
    private void ConfirmQuit()
    {
        gameManager.HasUnsavedChanges = false;
        
        if (isExit)
        {
            gameManager.MainSceneConfirmQuit();
        }
        else
        {
            gameManager.GotoMainMenu();
        } 
        
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임 종료인지 메인 메뉴 이동인지 확인
    /// </summary>
    /// <param name="isExit"></param>
    public void CheckExitOrMainMenu(bool isExit)
    {
        this.isExit = isExit;
    }
    
}
