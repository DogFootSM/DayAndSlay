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
        transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임 종료 진행
    /// </summary>
    private void ConfirmQuit()
    {
        gameManager.HasUnsavedChanges = false;
        gameManager.ConfirmQuit();
    }
    
}
