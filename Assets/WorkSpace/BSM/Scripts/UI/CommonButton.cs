using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public CommonCanvasType CanvasType;

    private Image buttonBackgroundImage;
    private TextMeshProUGUI buttonText;
    private Color buttonTextColor;
    
    [Inject]
    private CanvasManager canvasManager;
    
    private void Awake()
    {
        buttonBackgroundImage = GetComponent<Image>();
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonTextColor = buttonText.color;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonBackgroundImage.color = new Color(0.85f, 0.85f, 0.85f);
        
        buttonText.color = new Color(buttonTextColor.r, buttonTextColor.g, buttonTextColor.b, 0.75f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonBackgroundImage.color = new Color(1f, 1f, 1f);
        buttonText.color = new Color(buttonTextColor.r, buttonTextColor.g, buttonTextColor.b, 1f);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
         canvasManager.ChangeCanvas(CanvasType);
    }
 
}
