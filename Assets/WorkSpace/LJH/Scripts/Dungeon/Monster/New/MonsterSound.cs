using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum SoundType
{
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
    private AudioSource audioSource;
    [SerializeField] [SerializedDictionary] private SerializedDictionary<SoundType, AudioClip> audioClipDict;
    [SerializeField] private List<AudioClip> hitClips;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }



    public void PlaySFX(SoundType type)
    {
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
