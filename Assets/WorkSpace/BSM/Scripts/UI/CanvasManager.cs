using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    
    [SerializedDictionary("Canvas Type", "Canvas Object")] [SerializeField]
    private SerializedDictionary<CanvasType, GameObject> canvasDict; 
    private GameObject curCanvas;
    
    private Stack<GameObject> canvasStack = new Stack<GameObject>();
    private Dictionary<CanvasType, GameObject> menuDict = new Dictionary<CanvasType, GameObject>();
    
    private void Awake()
    { 
        Instantiate(mainCanvas);

        for (int i = 0; i < canvasDict.Count; i++)
        {
            menuDict[(CanvasType)i] = Instantiate(canvasDict[(CanvasType)i]);
        }
        
    }

    /// <summary>
    /// 캔버스 변경
    /// </summary>
    /// <param name="canvasType">활성화 할 캔버스 버튼 타입</param>
    public void ChangeCanvas(CanvasType canvasType)
    {
        if (canvasType == CanvasType.EXIT)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#else
            Application.Quit();

#endif
            return;
        }  
        
        canvasStack.Push(menuDict[canvasType]);
        
        menuDict[canvasType].SetActive(true);    
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