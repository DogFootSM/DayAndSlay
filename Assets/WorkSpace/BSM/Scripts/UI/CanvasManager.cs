using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializedDictionary("Canvas Type", "Canvas Object")] [SerializeField]
    private SerializedDictionary<CommonCanvasType, GameObject> canvasDict;

    public void ChangeCanvas(CommonCanvasType canvasType)
    {
        if (canvasType == CommonCanvasType.EXIT)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            
#else
            Application.Quit();

#endif
            return;
        }
        
        canvasDict[canvasType].SetActive(true);
        
    }
}