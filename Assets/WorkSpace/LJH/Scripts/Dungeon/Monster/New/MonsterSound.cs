using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public enum SoundType
{
    NOTUSED,
    ATTACK,
    HIT,
    DAMAGE,
    DEATH,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4,
}
public class MonsterSound : MonoBehaviour
{

    [SerializeField] [SerializedDictionary] private SerializedDictionary<SoundType, AudioClip> audioClipDict;
    [SerializeField] private List<AudioClip> hitClips;

    public static UnityEvent<bool> OnChangedMute = new UnityEvent<bool>();                  
    public static UnityEvent<float> OnChangedVolume = new UnityEvent<float>();               
    
    private SoundManager soundManager => SoundManager.Instance;
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnChangedMute.AddListener(SetMonsterMute);
        OnChangedVolume.AddListener(SetMonsterVolume);
    }

    private void OnDisable()
    {
        OnChangedMute.RemoveListener(SetMonsterMute);
        OnChangedVolume.RemoveListener(SetMonsterVolume);
    }
    
    /// <summary>
    /// SoundManager Mute 값에 따라 Monster Audio Source Mute 설정도 변경 
    /// </summary>
    /// <param name="mute"></param>
    private void SetMonsterMute(bool mute)
    {
        audioSource.mute = mute;
    }

    /// <summary>
    /// SoundManager Master Volume * Sfx Volume 값에 따라 Monster Audio Source Mute 설정도 변경 
    /// </summary>
    /// <param name="volume"></param>
    private void SetMonsterVolume(float volume)
    {
        audioSource.volume = volume;
    }
    
    
    public void PlaySFX(SoundType type)
    {
        //몬스터가 죽은 경우 예외 처리
        if (audioSource == null) return;
        if (!this) return;   
        
        if (type == SoundType.HIT)
        {
            audioSource.clip = hitClips[Random.Range(0, hitClips.Count)];
        }
        else
        {
            audioSource.clip = audioClipDict[type];
        }

        audioSource.PlayOneShot(audioSource.clip, 0.6F);
    }
}
