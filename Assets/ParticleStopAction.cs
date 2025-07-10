using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStopAction : MonoBehaviour
{
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    
    
    private void OnParticleSystemStopped()
    {
        instance.ReturnSkillParticlePool();
    }
}
