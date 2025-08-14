using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStopAction : MonoBehaviour
{
    public string SkillID;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;

    /// <summary>
    /// 파티클 정지 시 풀에 반환 이벤트
    /// </summary>
    private void OnParticleSystemStopped()
    {
        Debug.Log("종료");
        instance.ReturnSkillParticlePool(SkillID, gameObject);
    }
}
