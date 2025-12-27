using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<AudioSource> audioSources;

    public void PlaySound()
    {
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioSources[i].clip = audioClips[i];
            audioSources[i].Play();
        }
    }
}
