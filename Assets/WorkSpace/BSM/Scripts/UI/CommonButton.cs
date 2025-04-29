using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CommonButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public ButtonType ButtonType;

    private Image buttonBackgroundImage;
    private TextMeshProUGUI buttonText;
    private Color buttonTextColor;

    [Inject] private CanvasManager canvasManager;

    private void Awake()
    {
        buttonBackgroundImage = GetComponent<Image>();
        buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttonTextColor = buttonText.color;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonBackgroundColorChange(0.85f, 0.85f, 0.85f);
        ButtonTextAlphaChange(0.75f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonBackgroundColorChange(1f, 1f, 1f);
        ButtonTextAlphaChange(1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: 버튼 클릭 애니메이션으로 변경
        ButtonBackgroundColorChange(1f, 1f, 1f);
        ButtonTextAlphaChange(1f);
        canvasManager.ChangeCanvas(ButtonType);
    }
    
    /// <summary>
    /// 버튼 컬러 변경
    /// </summary>
    /// <param name="r">컬러의 r값</param>
    /// <param name="g">컬러의 g값</param>
    /// <param name="b">컬러의 b값</param>
    private void ButtonBackgroundColorChange(float r, float g, float b)
    {
        buttonBackgroundImage.color = new Color(r, g, b);
    }

    /// <summary>
    /// 버튼 투명도 변경
    /// </summary>
    /// <param name="alpha">버튼 색상의 알파값</param>
    private void ButtonTextAlphaChange(float alpha)
    {
        buttonText.color = new Color(buttonTextColor.r, buttonTextColor.g, buttonTextColor.b, alpha);
    }
    
}