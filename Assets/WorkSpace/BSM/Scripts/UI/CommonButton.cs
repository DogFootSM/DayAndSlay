using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CommonButton : MonoBehaviour, IPointerClickHandler
{
    public CanvasType canvasType;
    
    [Inject] private CanvasManager canvasManager;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        canvasManager.ChangeCanvas(canvasType);
    }
}