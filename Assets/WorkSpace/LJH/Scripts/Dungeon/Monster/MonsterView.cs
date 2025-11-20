using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MonsterView : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;

    
    public UnityEvent<float> OnChangeHealth;
   
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
    private void UpdateHealth(float hpValue)
    {
        healthBarSlider.value = hpValue;
    }

    
}