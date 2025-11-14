using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private TextMeshProUGUI _masterVolumeValueText;
    [SerializeField] private TextMeshProUGUI _bgmVolumeValueText;
    [SerializeField] private TextMeshProUGUI _sfxVolumeValueText;
    
    
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
        
        _masterVolumeValueText.text = $"{(int)(masterVolumeSlider.value * 100)}%";
        _bgmVolumeValueText.text = $"{(int)(bgmVolumeSlider.value * 100)}%";
        _sfxVolumeValueText.text = $"{(int)(sfxVolumeSlider.value * 100)}%";
    }

    /// <summary>
    /// Slider Value값 변화에 따른 Mixer Volume 설정 
    /// </summary>
    private void OnValueChanged()
    {
        masterVolumeSlider.onValueChanged.AddListener(x =>
        {
            soundManager.SetMasterVolume(x);

            _masterVolumeValueText.text = $"{(int)(soundManager.GetMasterVolume() * 100)}%";
        });
        
        bgmVolumeSlider.onValueChanged.AddListener(x =>
        {
            soundManager.SetBgmVolume(x);
            _bgmVolumeValueText.text = $"{(int)(soundManager.GetBgmVolume() * 100)}%";
        });
        
        sfxVolumeSlider.onValueChanged.AddListener(x =>
        {
            soundManager.SetSFxVolume(x);
            _sfxVolumeValueText.text = $"{(int)(soundManager.GetSfxVolume() * 100)}%";
        });
    }
    
}
