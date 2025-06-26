using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SoundManager : MonoBehaviour
{
    [SerializedDictionary("SFX 사운드 타입","SFX 사운드 파일")] [SerializeField]
    private SerializedDictionary<SFXSound, AudioClip> sfxSounds;
    
    [SerializedDictionary("BGM 사운드 타입","BGM 사운드 파일")] [SerializeField]
    private SerializedDictionary<BGMSound, AudioClip> bgmSounds;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    
    [Inject] private DataManager dataManager;
    
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //오디오 설정 Data Load
        dataManager.LoadAudioData();
        PlayBGM(BGMSound.START_SCENE_BGM);
    }

    /// <summary>
    /// 마스터 볼륨 설정
    /// </summary>
    /// <param name="volume">Audio Mixer MASTER 설정할 값</param>
    public void SetMasterVolume(float volume)
    { 
        audioMixer.SetFloat("MASTER", 20f * Mathf.Log10(volume)); 
    }

    /// <summary>
    /// 현재 설정된 Master Volume의 값 반환
    /// </summary>
    /// <returns>Master Volume Slider의 Value 값으로 사용</returns>
    public float GetMasterVolume()
    {
        audioMixer.GetFloat("MASTER", out float volume);  
        
        return Mathf.Pow(10f, volume / 20f); 
    }
    
    /// <summary>
    /// 배경음 볼륨 설정
    /// </summary>
    /// <param name="volume">Audio Mixer BGM 설정할 값</param>
    public void SetBgmVolume(float volume)
    {
        audioMixer.SetFloat("BGM", 20f * Mathf.Log10(volume));
    }

    /// <summary>
    /// 현재 설정된 BGM Mixer Volume의 값 반환
    /// </summary>
    /// <returns>BGM Volume Slider의 Value 값으로 사용</returns>
    public float GetBgmVolume()
    {
        audioMixer.GetFloat("BGM", out float volume);

        return Mathf.Pow(10f, volume / 20f);
    }
    
    /// <summary>
    /// 효과음 볼륨 설정
    /// </summary>
    /// <param name="volume">Audio Mixer SFX 설정할 값</param>
    public void SetSFxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", 20f * Mathf.Log10(volume));   
    }

    /// <summary>
    /// 현재 설정된 SFX Mixer Volume의 값 반환
    /// </summary>
    /// <returns>SFX Volume Slider의 Value 값으로 사용</returns>
    public float GetSfxVolume()
    {
        audioMixer.GetFloat("SFX", out float volume);

        return Mathf.Pow(10f, volume / 20f);
    }
    
    /// <summary>
    /// 효과음 1회 재생
    /// </summary>
    /// <param name="sfxSound">효과음으로 사용할 오디오 타입</param>
    public void PlaySfx(SFXSound sfxSound)
    {
        sfxAudioSource.PlayOneShot(sfxSounds[sfxSound]);
    }

    /// <summary>
    /// 배경음 재생
    /// </summary>
    /// <param name="bgmSound">배경음으로 사용할 오디오 타입</param>
    public void PlayBGM(BGMSound bgmSound)
    {
        bgmAudioSource.clip = bgmSounds[bgmSound];
        bgmAudioSource.Play();
    }
    
    
}
