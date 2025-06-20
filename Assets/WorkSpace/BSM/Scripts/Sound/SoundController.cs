using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    private SoundManager soundManager => SoundManager.Instance;

    private void Start()
    {
        SetSliderValue();
        OnValueChanged();
    }
 
    /// <summary>
    /// 설정된 사운드 값으로 Slider Value 설정
    /// </summary>
    private void SetSliderValue()
    {
        masterVolumeSlider.value = soundManager.GetMasterVolume();
        bgmVolumeSlider.value = soundManager.GetBgmVolume();
        sfxVolumeSlider.value = soundManager.GetSfxVolume();
    }

    /// <summary>
    /// Slider Value값 변화에 따른 Mixer Volume 설정 
    /// </summary>
    private void OnValueChanged()
    {
        masterVolumeSlider.onValueChanged.AddListener(x => soundManager.SetMasterVolume(x));
        bgmVolumeSlider.onValueChanged.AddListener(x => soundManager.SetBgmVolume(x));
        sfxVolumeSlider.onValueChanged.AddListener(x => soundManager.SetSFxVolume(x));
    }
    
}
