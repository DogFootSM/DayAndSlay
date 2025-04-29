using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializedDictionary("Canvas Type", "Canvas Object")] [SerializeField]
    private SerializedDictionary<ButtonType, GameObject> canvasDict;

    private GameObject curCanvas;
    
    /// <summary>
    /// 캔버스 변경
    /// </summary>
    /// <param name="buttonType">활성화 할 캔버스 버튼 타입</param>
    public void ChangeCanvas(ButtonType buttonType)
    {
        if (buttonType == ButtonType.EXIT)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#else
            Application.Quit();

#endif
            return;
        } 
        
        curCanvas = canvasDict[buttonType];
        
        canvasDict[buttonType].SetActive(true);    
    }

    /// <summary>
    /// 캔버스 닫기
    /// </summary>
    public void CloseCanvas()
    {
        curCanvas.SetActive(false);
    }
    
}