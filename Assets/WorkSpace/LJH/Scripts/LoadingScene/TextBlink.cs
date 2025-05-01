using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    private Color curColor;

    float Tick = 0.01f;
    float LowCut = 0.25f;
    float HighCut = 0.99f;

    TextMeshProUGUI loadingText;

    void Start()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
        curColor = loadingText.color;

        StartCoroutine(BlinkCoroutine());
    }


    IEnumerator BlinkCoroutine()
    {
        bool isDecreasing = true;

        //실제로 들어갈 값
        float tick;

        while (true)
        {
            yield return new WaitForSeconds(Tick);

            if (isDecreasing)
            {
                tick = -Tick;
                if (curColor.a <= LowCut)
                {
                    isDecreasing = !isDecreasing;
                }
            }
            else
            {
                tick = Tick;
                if (curColor.a >= HighCut)
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


