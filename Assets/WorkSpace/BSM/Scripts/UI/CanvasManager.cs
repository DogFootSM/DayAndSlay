using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializedDictionary("Canvas Type", "Canvas Object")] [SerializeField]
    private SerializedDictionary<CanvasType, GameObject> canvasDict; 
    private GameObject curCanvas;
    
    private Stack<GameObject> canvasStack = new Stack<GameObject>();
  
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
          
        canvasStack.Push(canvasDict[canvasType]);
        
        canvasDict[canvasType].SetActive(true);    
    }
    
    /// <summary>
    /// 로딩 캔버스 활성화
    /// </summary>
    /// <param name="scene">이동할 씬</param>
    public void OnActiveLoadingCanvas(SceneReference scene)
    {
        canvasDict[CanvasType.LOADING].SetActive(true);
        Loading.LoadScene(scene); 
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