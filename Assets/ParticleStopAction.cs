using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStopAction : MonoBehaviour
{
    public string SkillID;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    
    
    private void OnParticleSystemStopped()
    {
        instance.ReturnSkillParticlePool(SkillID, gameObject);
    }
}
