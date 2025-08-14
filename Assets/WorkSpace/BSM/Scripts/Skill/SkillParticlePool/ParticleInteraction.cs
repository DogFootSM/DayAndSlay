using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleInteraction : MonoBehaviour
{
    [FormerlySerializedAs("SkillID")] public string EffectId;
    
    private SkillParticlePooling instance => SkillParticlePooling.Instance;
    private List<Action> actions = new List<Action>();
    
    /// <summary>
    /// 실행할 스킬 효과를 받아옴
    /// </summary>
    /// <param name="actions">실행할 스킬 효과 함수들</param>
    public void ReceiveAction(List<Action> actions)
    {
        this.actions = actions;
    }
    
    /// <summary>
    /// 파티클 정지 시 풀에 반환 이벤트
    /// </summary>
    private void OnParticleSystemStopped()
    { 
        instance.ReturnSkillParticlePool(EffectId, gameObject);
    }

    private void OnParticleTrigger()
    {
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
}
