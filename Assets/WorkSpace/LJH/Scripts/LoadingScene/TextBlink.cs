using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    private Color curColor;
    private Color originColor;


    void Start()
    {
        curColor = gameObject.GetComponent<TextMeshProUGUI>().color;

        StartCoroutine(BlinkCoroutine());
    }


    IEnumerator BlinkCoroutine()
    {
        bool isDecreasing = true;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (isDecreasing)
            {
                MinusAlpha();
                if (curColor.a <= 0.25f)
                {
                    isDecreasing = false;
                }
            }
            else
            {
                PlusAlpha();
                if (curColor.a >= 0.99f)
                {
                    isDecreasing = true;
                }
            }
        }
    }


    void MinusAlpha()
    {
        curColor.a -= 0.01f;
        gameObject.GetComponent<TextMeshProUGUI>().color = curColor;
    }

    void PlusAlpha()
    {
        curColor.a += 0.01f;
        gameObject.GetComponent<TextMeshProUGUI>().color = curColor;
    }
}


