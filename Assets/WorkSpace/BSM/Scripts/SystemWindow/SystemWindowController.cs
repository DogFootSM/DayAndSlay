using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SystemWindowController : MonoBehaviour
{
    [SerializedDictionary("WindowType", "WindowCanvas")] [SerializeField]
    private SerializedDictionary<SystemType, GameObject> systemWindows;

    [SerializeField] private GameObject parentCanvas;

    private Stack<GameObject> canvasStack = new Stack<GameObject>();
    private SystemType currentSystemType;
    
    private void Update()
    {
        InputSystemKey();
    }

    /// <summary>
    /// 시스템 창 오픈
    /// </summary>
    /// <param name="systemType">시스템 창 종류</param>
    public void OpenSystemWindow(SystemType systemType)
    {
        GameObject openWindow = systemWindows[systemType];
        currentSystemType = systemType;
        
        if (canvasStack.Count == 0)
        {
            canvasStack.Push(parentCanvas);
            parentCanvas.SetActive(true);
        }
        
        if (canvasStack.Peek().Equals(openWindow))
        {
            AllCloseSystemWindows();
        }
        else
        {
            SwitchSystemWindows(openWindow);
        } 
    }

    /// <summary>
    /// 시스템 창 종료
    /// </summary>
    private void AllCloseSystemWindows()
    {
        while (canvasStack.Count != 0)
        {
            canvasStack.Pop().SetActive(false);
        } 
    }

    /// <summary>
    /// 시스템 창 변경
    /// </summary>
    /// <param name="switchWindow">변경할 시스템 창</param>
    private void SwitchSystemWindows(GameObject switchWindow)
    {
        if (canvasStack.Count > 1)
        {
            canvasStack.Pop().SetActive(false);
        }
        
        canvasStack.Push(switchWindow);
        switchWindow.SetActive(true);
    }
    
    /// <summary>
    /// 시스템 키 입력 감지
    /// </summary>
    private void InputSystemKey()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OpenSystemWindow(SystemType.SKILL);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            OpenSystemWindow(SystemType.STATUS);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            OpenSystemWindow(SystemType.RECIPE);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSystemWindow(SystemType.SETTING);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            OpenSystemWindow(SystemType.INVENTORY);
        }
    }

    public SystemType GetSystemType()
    {
        return currentSystemType;
    }
    
}