using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializedDictionary("Canvas Type", "Canvas Object")] [SerializeField]
    private SerializedDictionary<MenuType, GameObject> canvasDict; 
    private GameObject curCanvas;
    
    private Stack<GameObject> canvasStack = new Stack<GameObject>();
    
    /// <summary>
    /// 캔버스 변경
    /// </summary>
    /// <param name="menuType">활성화 할 캔버스 버튼 타입</param>
    public void ChangeCanvas(MenuType menuType)
    {
        if (menuType == MenuType.EXIT)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#else
            Application.Quit();

#endif
            return;
        }  
        
        canvasStack.Push(canvasDict[menuType]);
        
        canvasDict[menuType].SetActive(true);    
    }

    /// <summary>
    /// 캔버스 닫기
    /// </summary>
    public void CloseCanvas()
    { 
        if (canvasStack.Count > 0)
        { 
            curCanvas = canvasStack.Pop();
            curCanvas.SetActive(false);
        } 
    }
    
}