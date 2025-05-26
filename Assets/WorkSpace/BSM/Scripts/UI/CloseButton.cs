using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Inject] private CanvasManager canvasManager;
    
    private Button closeButton;
    private Image buttonBackgroundImage;
    private Image closeButtonImage;
    private Color curCloseButtonColor;
    
    private void Start()
    {
        closeButton = GetComponent<Button>();
        buttonBackgroundImage = GetComponent<Image>();
        closeButtonImage = transform.GetChild(0).GetComponent<Image>();
        
        curCloseButtonColor = closeButtonImage.color;
        
        closeButton.onClick.AddListener(CloseCanvas);
    }

    private void CloseCanvas()
    {
        canvasManager.CloseCanvas();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonBackgroundColorChange(0.85f, 0.85f, 0.8f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonBackgroundColorChange(1f, 1f, 1f);
        ButtonAlphaChange(0.75f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonBackgroundColorChange(1f, 1f, 1f);
        ButtonAlphaChange(1f);
    }

    /// <summary>
    /// 버튼 배경 색 변경
    /// </summary>
    /// <param name="r">변경할 버튼의 R색</param>
    /// <param name="g">변경할 버튼의 G색</param>
    /// <param name="b">변경할 버튼의 B색</param>
    private void ButtonBackgroundColorChange(float r, float g, float b)
    {
        buttonBackgroundImage.color = new Color(r, g, b);
    }
    
    /// <summary>
    /// 버튼 투명도 변경
    /// </summary>
    /// <param name="alpha">버튼 색상의 알파값</param>
    private void ButtonAlphaChange(float alpha)
    {
        closeButtonImage.color = new Color(curCloseButtonColor.r, curCloseButtonColor.g, curCloseButtonColor.b, alpha);
    }
    
}
