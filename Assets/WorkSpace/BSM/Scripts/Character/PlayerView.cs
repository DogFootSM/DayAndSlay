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

    [SerializeField] private Slider expBarSlider;
    [SerializeField] private TextMeshProUGUI expPerText;
    
    public UnityEvent<float, float, float> OnChangeHealth;
    public UnityEvent<float, float> OnChangeExp;
    
    private void OnEnable()
    {
        OnChangeHealth.AddListener(UpdateHealth);
        OnChangeExp.AddListener(UpdateExp);
    }

    private void OnDisable()
    {
        OnChangeHealth.RemoveListener(UpdateHealth);
        OnChangeExp.RemoveListener(UpdateExp);
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

    /// <summary>
    /// 현재 경험치에 따른 UI 업데이트
    /// </summary>
    /// <param name="curExp">현재 경험치</param>
    /// <param name="maxExp">레발당 최대 경험치</param>
    private void UpdateExp(float curExp, float maxExp)
    {
        expBarSlider.value = curExp / maxExp;
        expPerText.text = $"{(curExp / maxExp) * 100}%";
    }
    
}
