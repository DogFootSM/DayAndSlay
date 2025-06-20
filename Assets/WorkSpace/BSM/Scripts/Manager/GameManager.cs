using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private DataManager dataManager;

    public static GameManager Instance;

    private int windowMode;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dataManager.LoadDisplayData();
    }

    public void SetWindowMode(int windowMode)
    {
        //Windowed - 창모드 3
        //Exclusive Fullscreen - 전체 화면 0
        //Fullscreen Window - 테두리 없는 창모드 1
        Screen.fullScreenMode = (FullScreenMode)windowMode;
        
        this.windowMode = windowMode;
    }

    public int GetWindowMode()
    { 
        return windowMode;
    }
    
    public void SetResolution(int resolution)
    {
        
    }

    /// <summary>
    /// 마우스 커서 윈도우 잠금 설정
    /// </summary>
    /// <param name="mouseCursorLockMode">0 설정 시 잠금 해제, 2 설정 시 윈도우 마우스 잠금</param>
    public void SetMouseCursorLockMode(int mouseCursorLockMode)
    {
        Cursor.lockState = (CursorLockMode)mouseCursorLockMode;
    }
    
    
}
