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
    
    private SoundManager soundManager => SoundManager.Instance;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        soundManager.PlaySfx(SFXSound.UI_BUTTON_CLICK);

        if (canvasType != CanvasType.SIZE)
        {
            canvasManager.ChangeCanvas(canvasType);
        } 
    }
}