using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    private Color curColor;

    [Header("텍스트 색상 변경 속도 조절 default: 0.01f")]
    [SerializeField] private float tick = 0.01f;
    private float lowCut = 0.25f;
    private float highCut = 0.99f;

    private TextMeshProUGUI loadingText;

    private Coroutine blinkTextCo;
    
    void Awake()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    { 
        curColor = loadingText.color;

        if (blinkTextCo == null)
        {
            blinkTextCo = StartCoroutine(BlinkCoroutine());
        } 
    }

    private void OnDisable()
    {
        if (blinkTextCo != null)
        {
            StopCoroutine(blinkTextCo);
            blinkTextCo = null;
        }
    }

    IEnumerator BlinkCoroutine()
    {
        bool isDecreasing = true;

        //실제로 들어갈 값
        float tick;

        while (true)
        {
            yield return new WaitForSeconds(this.tick);

            if (isDecreasing)
            {
                tick = -this.tick;
                if (curColor.a <= lowCut)
                {
                    isDecreasing = !isDecreasing;
                }
            }
            else
            {
                tick = this.tick;
                if (curColor.a >= highCut)
                {
                    isDecreasing = !isDecreasing;
                }
            }

            ChangeAlpha(tick);
        }
    }

    void ChangeAlpha(float num)
    {
        curColor.a += num;
        loadingText.color = curColor;
    }

}


