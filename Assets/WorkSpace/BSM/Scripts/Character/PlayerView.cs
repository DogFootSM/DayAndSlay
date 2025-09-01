using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    
    public UnityEvent<float, float, float> OnChangeHealth;

    private void OnEnable()
    {
        OnChangeHealth.AddListener(UpdateHealth);
    }

    private void OnDisable()
    {
        OnChangeHealth.RemoveListener(UpdateHealth);
    }
    
    /// <summary>
    /// 현재 체력에 따른 체력바 업데이트
    /// </summary>
    /// <param name="hpValue"></param>
    private void UpdateHealth(float hpValue, float curHealth, float maxHealth)
    {
        healthBarSlider.value = hpValue;
        healthText.text = $"{(int)curHealth} / {(int)maxHealth}";
    }

}
