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
        Init();
    }

    private void Init()
    {
        masterVolumeSlider.value = soundManager.GetMasterVolume();
        bgmVolumeSlider.value = soundManager.GetBgmVolume();
        sfxVolumeSlider.value = soundManager.GetSfxVolume();
        
        masterVolumeSlider.onValueChanged.AddListener(x => soundManager.SetMasterVolume(x));
        bgmVolumeSlider.onValueChanged.AddListener(x => soundManager.SetBgmVolume(x));
        sfxVolumeSlider.onValueChanged.AddListener(x => soundManager.SetSFxVolume(x));
    }
    
}
