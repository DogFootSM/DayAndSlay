using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> canvasList = new List<GameObject>();


    public void ChangeCanvas(CommonCanvasType canvasType)
    {
        Debug.Log(canvasType);
    }
    
    
}
