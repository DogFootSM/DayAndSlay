using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private DataManager dataManager;
    
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
        uint uFlags);
    
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
    
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);
     
    public static GameManager Instance;
    
    private const int GWL_STYLE = -16;
    private const uint WS_POPUP = 0x80000000;
    private const uint WS_VISIBLE = 0x10000000;
    private const uint SWP_SHOWWINDOW = 0x0040;
    private const uint WS_OVERLAPPEDWINDOW = 0x00CF0000;
    
    private Coroutine borderlessCo;
    
    private int windowMode;
    private (int, int) resolution;
    private int resolutionIndex;
    
    private Dictionary<int, (int, int)> resolutionMaps = new()
    {
        {0,(1920, 1080)},
        {1,(1600, 900)},
        {2,(1366, 768)},
        {3,(1280, 720)}, 
    };
    
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
 
    /// <summary>
    /// 사용자가 설정한 화면 모드에 따른 화면 모드 설정
    /// </summary>
    /// <param name="windowMode"></param>
    public void SetWindowMode(int windowMode)
    {
        Screen.fullScreenMode = (FullScreenMode)windowMode;

        this.windowMode = windowMode;

        switch (windowMode)
        {
            //전체 화면 선택시에는 1920, 1080 전체 화면으로 고정
            case 0 :
                SetResolution(windowMode);
                break;
            
            //테두리 없는 창모드일 경우 현재 화면 해상도 인덱스로 설정
            case 1 :
                SetResolution(resolutionIndex);
                break;
            
            //테두리 없는 창모드 -> 창모드 변경 시 테두리 복구
            default :
                Borderless();
                break;
        } 
    }

    /// <summary>
    /// 화면 모드 토글 업데이트에 사용할 현재 사용중인 윈도우 모드 상태
    /// </summary>
    /// <returns></returns>
    public int GetWindowMode()
    {  
        return windowMode;
    }
    
    /// <summary>
    /// 선택한 옵션에 따른 화면 해상도 설정
    /// </summary>
    /// <param name="resolution">드롭다운 메뉴로 선택한 옵션값</param>
    public void SetResolution(int resolution)
    {
        resolutionIndex = resolution;
        
        if (borderlessCo != null)
        {
            StopCoroutine(borderlessCo);
            borderlessCo = null;
        }
        
        this.resolution = resolution switch
        {
            0 => resolutionMaps[0],
            1 => resolutionMaps[1],
            2 => resolutionMaps[2],
            3 => resolutionMaps[3]
        };

        bool isFullScreen = (FullScreenMode)windowMode == FullScreenMode.ExclusiveFullScreen;
         
        if ((FullScreenMode)windowMode == FullScreenMode.FullScreenWindow)
        {
            SetBorderlessResolution(this.resolution.Item1, this.resolution.Item2); 
        }
        else
        {
            Screen.SetResolution(this.resolution.Item1, this.resolution.Item2, isFullScreen);
        }
        
    }

    /// <summary>
    /// 화면 해상도 설정 후 테두리 제거 호출
    /// </summary>
    /// <param name="width">설정할 해상도의 가로</param>
    /// <param name="height">설정할 해상도의 높이</param>
    private void SetBorderlessResolution(int width, int height)
    {
        Screen.SetResolution(width, height, false);

        if (borderlessCo == null)
        {
            borderlessCo = StartCoroutine(BorderlessRoutine());
        } 
    }
    
    /// <summary>
    /// 마우스 커서 윈도우 잠금 설정
    /// </summary>
    /// <param name="mouseCursorLockMode">0 설정 시 잠금 해제, 2 설정 시 윈도우 마우스 잠금</param>
    public void SetMouseCursorLockMode(int mouseCursorLockMode)
    {
        Cursor.lockState = (CursorLockMode)mouseCursorLockMode;
    }

    /// <summary>
    /// 현재 커서 상태 반환
    /// </summary>
    /// <returns>커서 토글 업데이트에 사용할 현재 커서 상태</returns>
    public int GetMouseCursorLockMode()
    {
        return (int)Cursor.lockState;
    }

    private IEnumerator BorderlessRoutine()
    {
        yield return WaitCache.GetWait(0.1f);
        Borderless();
    }
    
    /// <summary>
    /// 테두리 없는 창모드 해상도 변경 시 테두리 제거
    /// </summary>
    private void Borderless()
    {
        var hwnd = GetActiveWindow();
        
        //창모드 변경 시 테두리 복구
        if (windowMode == 3)
        {
            SetWindowLong(hwnd, GWL_STYLE, WS_OVERLAPPEDWINDOW | WS_VISIBLE);
        }
        else
        {
            SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        }
         
        int width = resolution.Item1;
        int height = resolution.Item2;

        int screenWidth = GetSystemMetrics(0);
        int screenHeight = GetSystemMetrics(1);

        int posX = (screenWidth - width) / 2;
        int posY = (screenHeight - height) / 2;
        
        SetWindowPos(hwnd,0, posX, posY, width, height, SWP_SHOWWINDOW); 
    }

    
}
