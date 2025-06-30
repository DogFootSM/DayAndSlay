using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


public class AspectRatioController : MonoBehaviour
{
    private float aspectRatioWidth  = 16;
    private float aspectRatioHeight = 9;
    
    private int minWidthPixel  = 1280;
    private int minHeightPixel = 720;
    private int maxWidthPixel  = 1920;
    private int maxHeightPixel = 1080;
 
    private float aspect;                   // 사용할 화면 비율 16:9 
    
    private int setWidth  = -1;
    private int setHeight = -1;
 
    private const int WM_SIZING = 0x214;
 
    private const int WMSZ_LEFT    = 1;
    private const int WMSZ_RIGHT   = 2;
    private const int WMSZ_TOP     = 3;
    private const int WMSZ_BOTTOM  = 6;
 
    private const int GWLP_WNDPROC = -4;
 
    private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    private WndProcDelegate wndProcDelegate;
 
    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();
 
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
 
    [DllImport("user32.dll")]
    private static extern bool EnumThreadWindows(uint dwThreadId, EnumWindowsProc lpEnumFunc, IntPtr lParam);
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
 
    [DllImport("user32.dll")]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
 
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);
 
    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, ref RECT lpRect);
 
    [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
    private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
 
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
 
    private const string UNITY_WND_CLASSNAME = "UnityWndClass";
 
    private IntPtr unityHWnd;
 
    private IntPtr oldWndProcPtr;
 
    private IntPtr newWndProcPtr;
 
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    } 
    
    private void Start()
    {
        // 에디터에서 실행 방지
        if (Application.isEditor)
        {
            return;
        }
         
        EnumThreadWindows(GetCurrentThreadId(), (hWnd, lParam) =>
        {
            var classText = new StringBuilder(UNITY_WND_CLASSNAME.Length + 1);
            GetClassName(hWnd, classText, classText.Capacity);

            if (classText.ToString() == UNITY_WND_CLASSNAME)
            {
                unityHWnd = hWnd;
                return false;
            }
            return true;
        }, IntPtr.Zero);

        //현재 해상도에 대한 화면비 적용
        aspect = aspectRatioWidth / aspectRatioHeight;
        
        wndProcDelegate = wndProc;
        newWndProcPtr = Marshal.GetFunctionPointerForDelegate(wndProcDelegate);
        oldWndProcPtr = SetWindowLong(unityHWnd, GWLP_WNDPROC, newWndProcPtr);
    }
   
    IntPtr wndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    { 
        if (msg == WM_SIZING)
        { 
            RECT rc = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
 
            RECT windowRect = new RECT();
            GetWindowRect(unityHWnd, ref windowRect);

            RECT clientRect = new RECT();
            GetClientRect(unityHWnd, ref clientRect);

            int borderWidth = windowRect.Right - windowRect.Left - (clientRect.Right - clientRect.Left);
            int borderHeight = windowRect.Bottom - windowRect.Top - (clientRect.Bottom - clientRect.Top);
 
            rc.Right -= borderWidth;
            rc.Bottom -= borderHeight;
 
            int newWidth = Mathf.Clamp(rc.Right - rc.Left, minWidthPixel, maxWidthPixel);
            int newHeight = Mathf.Clamp(rc.Bottom - rc.Top, minHeightPixel, maxHeightPixel);
 
            switch (wParam.ToInt32())
            {
                case WMSZ_LEFT:
                    rc.Left = rc.Right - newWidth;
                    rc.Bottom = rc.Top + Mathf.RoundToInt(newWidth / aspect);
                    break;
                case WMSZ_RIGHT:
                    rc.Right = rc.Left + newWidth;
                    rc.Bottom = rc.Top + Mathf.RoundToInt(newWidth / aspect);
                    break;
                case WMSZ_TOP:
                    rc.Top = rc.Bottom - newHeight;
                    rc.Right = rc.Left + Mathf.RoundToInt(newHeight * aspect);
                    break;
                case WMSZ_BOTTOM:
                    rc.Bottom = rc.Top + newHeight;
                    rc.Right = rc.Left + Mathf.RoundToInt(newHeight * aspect);
                    break;
                case WMSZ_RIGHT + WMSZ_BOTTOM:
                    rc.Right = rc.Left + newWidth;
                    rc.Bottom = rc.Top + Mathf.RoundToInt(newWidth / aspect);
                    break;
                case WMSZ_RIGHT + WMSZ_TOP:
                    rc.Right = rc.Left + newWidth;
                    rc.Top = rc.Bottom - Mathf.RoundToInt(newWidth / aspect);
                    break;
                case WMSZ_LEFT + WMSZ_BOTTOM:
                    rc.Left = rc.Right - newWidth;
                    rc.Bottom = rc.Top + Mathf.RoundToInt(newWidth / aspect);
                    break;
                case WMSZ_LEFT + WMSZ_TOP:
                    rc.Left = rc.Right - newWidth;
                    rc.Top = rc.Bottom - Mathf.RoundToInt(newWidth / aspect);
                    break;
            }
 
            setWidth = rc.Right - rc.Left;
            setHeight = rc.Bottom - rc.Top;
 
            rc.Right += borderWidth;
            rc.Bottom += borderHeight;
 
            Marshal.StructureToPtr(rc, lParam, true);
        }
 
        return CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);
    }
 
    private void Update()
    {  
        if (!Screen.fullScreen && setWidth != -1 && setHeight != -1 && (Screen.width != setWidth || Screen.height != setHeight))
        { 
            setHeight = Screen.height;
            setWidth = Mathf.RoundToInt(Screen.height * aspect);
             
            if (!IsAspectRatioValid(Screen.width, Screen.height))
            {
                Screen.SetResolution(setWidth, setHeight, Screen.fullScreen);
            } 
        }
         
        #if UNITY_EDITOR
        if (Screen.width != setWidth || Screen.height != setHeight)
        {
            setWidth = Screen.width;
            setHeight = Screen.height;    
        }
        #endif
    }

    private static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    { 
        if (IntPtr.Size == 4)
        {
            return SetWindowLong32(hWnd, nIndex, dwNewLong);
        }
        return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
    }
    
    /// <summary>
    /// 화면비 유효성 검사
    /// </summary>
    /// <param name="screenWidth">화면의 가로 길이</param>
    /// <param name="screenHeight">화면의 세로 길이</param>
    /// <returns></returns>
    private bool IsAspectRatioValid(int screenWidth, int screenHeight)
    {
        float currentAspectRatio = (float)screenWidth / screenHeight;
        float targetAspectRatio = (aspectRatioWidth / aspectRatioHeight);
        float tolerance = 0.01f;

        if (Mathf.Abs(currentAspectRatio - targetAspectRatio) > tolerance)
        {
            return false;
        }
        else
        { 
            return true;
        }
    }
}
