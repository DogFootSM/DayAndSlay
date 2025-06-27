using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DisplayController : MonoBehaviour
{
    [SerializeField] private List<Toggle> windowModeToggles;
    [SerializeField] private List<Toggle> mouseLockToggles;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    
    private GameManager gameManager => GameManager.Instance;

    private readonly int[] windowModeValues = new[]{ 0, 3, 1 };
    private readonly int[] mouseLockValues = new[]{ 2, 0 };
 
    private void Start()
    { 
        OnChangedWindowModeToggle();
        UpdateWindowModeToggles();
        OnChangedMouseLockToggle();
        UpdateMouseLockToggles();
        OnChangedResolution();
    }

    /// <summary>
    /// 토글 이벤트 등록
    /// </summary>
    private void OnChangedWindowModeToggle()
    {
        for (int i = 0; i < windowModeToggles.Count; i++)
        {
            int index = i;
            
            windowModeToggles[i].onValueChanged.AddListener(toggle =>
            {
                if(toggle) gameManager.SetWindowMode(windowModeValues[index]);

                UpdateWindowModeToggles();
            }); 
        } 
    }

    /// <summary>
    /// 현재 선택된 화면 모드 토글 외 선택 해제
    /// </summary>
    private void UpdateWindowModeToggles()
    {
        for (int i = 0; i < windowModeToggles.Count; i++)
        {
            windowModeToggles[i].isOn = gameManager.GetWindowMode() == windowModeValues[i];
        }

        
        resolutionDropdown.interactable = !windowModeToggles[0].isOn;
        
        if (windowModeToggles[0].isOn)
        { 
            resolutionDropdown.value = 0;
        } 
    }
    
    /// <summary>
    /// 마우스 가두기 토글 이벤트 등록
    /// </summary>
    private void OnChangedMouseLockToggle()
    {
        for (int i = 0; i < mouseLockToggles.Count; i++)
        {
            int index = i;
            
            mouseLockToggles[i].onValueChanged.AddListener(toggle =>
            {
                if(toggle) gameManager.SetMouseCursorLockMode(mouseLockValues[index]);
                
                UpdateMouseLockToggles();
            }); 
        } 
    }

    /// <summary>
    /// 마우스 가두기 토글 상태 업데이트
    /// </summary>
    private void UpdateMouseLockToggles()
    {
        for (int i = 0; i < mouseLockToggles.Count; i++)
        {
            mouseLockToggles[i].isOn = gameManager.GetMouseCursorLockMode() == mouseLockValues[i];
        }
    }

    /// <summary>
    /// 화면 해상도 이벤트 등록
    /// </summary>
    private void OnChangedResolution()
    { 
        resolutionDropdown.value = gameManager.GetResolution();
        resolutionDropdown.onValueChanged.AddListener(x => gameManager.SetResolution(x));
    }
    
}
