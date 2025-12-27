using System;
using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private SoundType soundType;
    private MonsterSound sound;

    private void Awake()
    {
        if (sound == null) sound = GetComponentInParent<MonsterSound>();
    }
    public void PlaySkill()
    {
        if (soundType != SoundType.NOTUSED)
        {
            sound.PlaySFX(soundType);
        }
        
        GetComponent<ParticleSystem>().Play();
    }

    //사운드매니저 
    //이펙트 소리들 사운드매니저에 넣음
    //여기에서 사운드매니저 접근하면서 소리 타입을 넘겨줌
    //그러면서 그 넘긴 타입에 따라 사운드 매니저를 통해서 클립을 다시 받아와서 소리를 재생
    
}