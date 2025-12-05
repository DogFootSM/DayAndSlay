using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
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

    private bool isMasterMute;
    private bool isBgmMute;
    private bool isSfxMute;
    
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
    /// 데이터 로드 시 뮤트 상태 초기화
    /// </summary>
    /// <param name="masterMute"></param>
    /// <param name="bgmMute"></param>
    /// <param name="sfxMute"></param>
    public void SetMuteState(bool masterMute, bool bgmMute, bool sfxMute)
    {
        
        isMasterMute = masterMute;
        isBgmMute = bgmMute;
        isSfxMute = sfxMute;

        SetMasterMute(isMasterMute);
        SetBgmMute(isBgmMute);
        SetSfxMute(isSfxMute);
    }
    
    /// <summary>
    /// 마스터 볼륨 뮤트 설정
    /// </summary> 
    public void SetMasterMute(bool mute)
    {
        isMasterMute = mute;
        
        if (isMasterMute)
        {
            bgmAudioSource.mute = true;
            sfxAudioSource.mute = true;
        }
        else
        {
            bgmAudioSource.mute = isBgmMute;
            sfxAudioSource.mute = isSfxMute;
        } 
    }
     
    /// <summary>
    /// BGM 볼륨 뮤트 설정
    /// </summary>
    public void SetBgmMute(bool mute)
    {
        //bgm뮤트 상태 업데이트
        isBgmMute = mute;
        
        if (isMasterMute) return;
        
        //마스터 볼륨 뮤트가 체크되어 있지 않을 경우 BGM 뮤트 업데이트
        bgmAudioSource.mute = mute;
    }

    /// <summary>
    /// SFX 볼륨 뮤트 설정
    /// </summary>
    /// <param name="mute"></param>
    public void SetSfxMute(bool mute)
    {
        //SFX 볼륨 상태 업데이트
        isSfxMute = mute;
        
        if (isMasterMute) return;
        
        //마스터 볼륨 뮤트가 체크되어 있지 않을 경우 SFX 뮤트 업데이트
        sfxAudioSource.mute = isSfxMute;
    }

    /// <summary>
    /// 현재 마스터 볼륨 뮤트 상태를 반환
    /// </summary>
    /// <returns></returns>
    public bool GetMasterMute()
    {
        return isMasterMute;
    }

    /// <summary>
    /// 현재 BGM 볼륨 뮤트 상태 반환
    /// </summary>
    /// <returns></returns>
    public bool GetBgmMute()
    {
        return isBgmMute;
    }

    /// <summary>
    /// 현재 SFX 볼륨 뮤트 상태 반환
    /// </summary>
    /// <returns></returns>
    public bool GetSfxMute()
    {
        return isSfxMute;
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

    /// <summary>
    /// 버튼 클릭에 따른 BGM Source Pause Or Play
    /// </summary>
    /// <param name="isPlay">BGM 재생 요청 혹은 퍼즈 요청 상태</param>
    public void PauseOrPlayBGM(bool isPlay)
    {
        if (isPlay)
        {
            bgmAudioSource.UnPause();
        }
        else
        {
            bgmAudioSource.Pause();
        } 
    }
}
