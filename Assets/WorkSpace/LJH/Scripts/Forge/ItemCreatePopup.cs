using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCreatePopup : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI text;
    private Animator animator;
    
    [SerializeField] float duration = 2f;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySfx(SFXSound.CREATE);
        StartCoroutine(SliderCoroutine());
        StartCoroutine(TextCoroutine());
        animator.Play("ForgeOkayButton");
    }


    private IEnumerator SliderCoroutine()
    {
        float timer = 0f;

        slider.value = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }

        slider.value = 1f;
    }


    private IEnumerator TextCoroutine()
    {
        float timer = 0f;

        Color baseColor = text.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(0.3f, 1f, Mathf.PingPong(timer * 2f, 1f));

            text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        // 마지막엔 원래 알파 복원
        text.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        
        //끝나면 창닫기
        gameObject.SetActive(false);
    }
}
